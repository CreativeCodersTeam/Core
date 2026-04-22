using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CreativeCoders.Core.Threading;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Collections;

/// <summary>
///     A thread-safe observable collection that implements <see cref="IList{T}"/>,
///     <see cref="IReadOnlyList{T}"/>, <see cref="INotifyPropertyChanged"/>,
///     and <see cref="INotifyCollectionChanged"/>. Supports batch updates, item movement,
///     and configurable synchronization context dispatch.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
[PublicAPI]
public class ExtendedObservableCollection<T> : IList<T>, IReadOnlyList<T>, INotifyPropertyChanged,
    INotifyCollectionChanged
{
    private const string IndexerName = "Item[]";

    private readonly SynchronizedValue<bool> _collectionHasChanged;

    private readonly List<T> _items;

    private readonly ILockingMechanism _lockingMechanism;

    private readonly SimpleMonitor _reentrancyMonitor;

    private readonly SynchronizationContext? _synchronizationContext;

    private readonly SynchronizationMethod _synchronizationMethod;

    private readonly SynchronizedValue<int> _updateCounter;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ExtendedObservableCollection{T}"/> class
    ///     that is empty, uses the current synchronization context with <see cref="SynchronizationMethod.Send"/>,
    ///     and a <see cref="LockSlimLockingMechanism"/>.
    /// </summary>
    public ExtendedObservableCollection()
        : this(SynchronizationContext.Current, SynchronizationMethod.Send,
            () => new LockSlimLockingMechanism(), []) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ExtendedObservableCollection{T}"/> class
    ///     that contains the specified items, uses the current synchronization context with
    ///     <see cref="SynchronizationMethod.Send"/>, and a <see cref="LockSlimLockingMechanism"/>.
    /// </summary>
    /// <param name="items">The initial elements to populate the collection with.</param>
    public ExtendedObservableCollection(IEnumerable<T> items)
        : this(SynchronizationContext.Current, SynchronizationMethod.Send,
            () => new LockSlimLockingMechanism(), items) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ExtendedObservableCollection{T}"/> class
    ///     that is empty, using the specified synchronization context, method, and locking mechanism.
    /// </summary>
    /// <param name="synchronizationContext">
    ///     The synchronization context for dispatching change notifications,
    ///     or <see langword="null"/> for no synchronization.
    /// </param>
    /// <param name="synchronizationMethod">The method used to dispatch notifications.</param>
    /// <param name="lockingMechanism">The factory that creates the locking mechanism for thread safety.</param>
    public ExtendedObservableCollection(SynchronizationContext? synchronizationContext,
        SynchronizationMethod synchronizationMethod, Func<ILockingMechanism> lockingMechanism)
        : this(synchronizationContext, synchronizationMethod, lockingMechanism, []) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ExtendedObservableCollection{T}"/> class
    ///     that contains the specified items, using the specified synchronization context, method,
    ///     and locking mechanism.
    /// </summary>
    /// <param name="synchronizationContext">
    ///     The synchronization context for dispatching change notifications,
    ///     or <see langword="null"/> for no synchronization.
    /// </param>
    /// <param name="synchronizationMethod">The method used to dispatch notifications.</param>
    /// <param name="lockingMechanism">The factory that creates the locking mechanism for thread safety.</param>
    /// <param name="items">The initial elements to populate the collection with.</param>
    public ExtendedObservableCollection(SynchronizationContext? synchronizationContext,
        SynchronizationMethod synchronizationMethod, Func<ILockingMechanism> lockingMechanism,
        IEnumerable<T> items)
    {
        Ensure.IsNotNull(synchronizationMethod);
        Ensure.IsNotNull(lockingMechanism);
        Ensure.IsNotNull(items);

        _synchronizationContext = synchronizationContext;
        _synchronizationMethod =
            _synchronizationContext != null ? synchronizationMethod : SynchronizationMethod.None;
        _lockingMechanism = lockingMechanism();

        _items = [..items];
        _updateCounter = SynchronizedValue.Create<int>(lockingMechanism());
        _collectionHasChanged = SynchronizedValue.Create<bool>(lockingMechanism());
        _reentrancyMonitor = new SimpleMonitor();
    }

    /// <summary>
    ///     Adds the elements of the specified sequence to the end of the collection
    ///     and raises a reset notification.
    /// </summary>
    /// <param name="items">The elements to add.</param>
    public void AddRange(IEnumerable<T> items)
    {
        _lockingMechanism.Write(() =>
        {
            CheckReentrancy();

            _items.AddRange(items);
        });

        NotifyItemsReset();
    }

    /// <summary>
    ///     Moves an element from one position to another within the collection.
    /// </summary>
    /// <param name="oldIndex">The zero-based index of the element to move.</param>
    /// <param name="newIndex">The zero-based destination index.</param>
    public void Move(int oldIndex, int newIndex)
    {
        var movedItem = _lockingMechanism.Write(() =>
        {
            Ensure.IndexIsInRange(oldIndex, _items.Count);
            Ensure.IndexIsInRange(newIndex, _items.Count);

            CheckReentrancy();

            var item = _items[oldIndex];
            _items.RemoveAt(oldIndex);
            _items.Insert(newIndex, item);

            return item;
        });

        NotifyItemMoved(movedItem, oldIndex, newIndex);
    }

    private void NotifyItemMoved(T item, int oldIndex, int newIndex)
    {
        InvokeSynchronizationContext(() =>
        {
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex,
                    oldIndex));
        });
    }

    /// <summary>
    ///     Begins a batch update scope. Change notifications are deferred until the returned
    ///     <see cref="IDisposable"/> is disposed.
    /// </summary>
    /// <returns>An <see cref="IDisposable"/> that ends the batch update when disposed.</returns>
    public IDisposable Update()
    {
        BeginUpdate();

        return new DelegateDisposable(EndUpdate, true);
    }

    /// <summary>
    ///     Begins a batch update, deferring change notifications until <see cref="EndUpdate"/> is called.
    ///     Multiple calls can be nested; notifications resume only when every <see cref="BeginUpdate"/>
    ///     has a matching <see cref="EndUpdate"/>.
    /// </summary>
    public void BeginUpdate() => _updateCounter.SetValue(x => x + 1);

    /// <summary>
    ///     Ends a batch update previously started with <see cref="BeginUpdate"/>. If this is the
    ///     outermost update and the collection changed during the batch, a reset notification is raised.
    /// </summary>
    [SuppressMessage("csharpsquid", "S2583", Justification = "Notify is set inside a lambda expression")]
    public void EndUpdate()
    {
        var notify = false;

        _updateCounter.SetValue(updateCounter =>
        {
            updateCounter--;

            if (updateCounter != 0 || !_collectionHasChanged.Value)
            {
                return updateCounter;
            }

            _collectionHasChanged.Value = false;
            notify = true;

            return updateCounter;
        });

        if (notify)
        {
            NotifyItemsReset();
        }
    }

    private void InvokeSynchronizationContext(Action execute)
    {
        using (BlockReentrancy())
        {
            DoSynchronization(_synchronizationMethod, execute);
        }
    }

    private void DoSynchronization(SynchronizationMethod synchronizationMethod, Action execute)
    {
        switch (synchronizationMethod)
        {
            case SynchronizationMethod.None:
                execute();
                break;
            case SynchronizationMethod.Post:
                if (_synchronizationContext == null)
                {
                    execute();
                }
                else
                {
                    _synchronizationContext.Post(_ => execute(), null);
                }

                break;
            case SynchronizationMethod.Send:
                if (_synchronizationContext == null)
                {
                    execute();
                }
                else
                {
                    _synchronizationContext.Send(_ => execute(), null);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(synchronizationMethod));
        }
    }

    private IDisposable BlockReentrancy()
    {
        _reentrancyMonitor.Enter();

        return _reentrancyMonitor;
    }

    private void CheckReentrancy()
    {
        if (_reentrancyMonitor.Busy && CollectionChanged != null &&
            CollectionChanged.GetInvocationList().Length > 1)
        {
            throw new InvalidOperationException("ExtendedObservableCollection reentrancy not allowed");
        }
    }

    private void NotifyItemAdded(T item, int index)
    {
        InvokeSynchronizationContext(() =>
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        });
    }

    private void NotifyItemRemoved(T item)
    {
        InvokeSynchronizationContext(() =>
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        });
    }

    private void NotifyItemRemoved(T item, int index)
    {
        InvokeSynchronizationContext(() =>
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        });
    }

    private void NotifyItemReplaced(T oldItem, T newItem, int index)
    {
        InvokeSynchronizationContext(() =>
        {
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                newItem, oldItem, index));
        });
    }

    private void NotifyItemsReset()
    {
        InvokeSynchronizationContext(() =>
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        });
    }

    /// <summary>
    ///     Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    ///     Raises the <see cref="CollectionChanged"/> event, or defers the notification
    ///     when a batch update is in progress.
    /// </summary>
    /// <param name="e">The event arguments describing the change.</param>
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (_updateCounter.Value > 0)
        {
            _collectionHasChanged.Value = true;
            return;
        }

        CollectionChanged?.Invoke(this, e);
    }

    /// <inheritdoc />
    [MustDisposeResource]
    public IEnumerator<T> GetEnumerator()
        => _lockingMechanism.Read([MustDisposeResource]() => _items.ToList().GetEnumerator());

    /// <inheritdoc />
    [MustDisposeResource]
    IEnumerator IEnumerable.GetEnumerator()
        => _lockingMechanism.Read([MustDisposeResource]() => _items.ToList().GetEnumerator());

    /// <inheritdoc />
    public void Add(T item)
    {
        var index = _lockingMechanism.Write(() =>
        {
            CheckReentrancy();

            _items.Add(item);

            return _items.Count - 1;
        });

        NotifyItemAdded(item, index);
    }

    /// <inheritdoc />
    public void Clear()
    {
        var itemsCleared = _lockingMechanism.Write(() =>
        {
            if (_items.Count == 0)
            {
                return false;
            }

            CheckReentrancy();

            _items.Clear();

            return true;
        });

        if (itemsCleared)
        {
            NotifyItemsReset();
        }
    }

    /// <inheritdoc />
    public bool Contains(T item) => _lockingMechanism.Read(() => _items.Contains(item));

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex)
    {
        _lockingMechanism.Read(() => Array.Copy(_items.ToArray(), 0, array, arrayIndex, _items.Count));
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
        var isRemoved = _lockingMechanism.Write(() =>
        {
            CheckReentrancy();

            var removed = _items.Remove(item);

            return removed;
        });

        if (isRemoved)
        {
            NotifyItemRemoved(item);
        }

        return isRemoved;
    }

    /// <inheritdoc />
    public int IndexOf(T item) => _lockingMechanism.Read(() => _items.IndexOf(item));

    /// <inheritdoc />
    public void Insert(int index, T item)
    {
        _lockingMechanism.Write(() =>
        {
            CheckReentrancy();

            _items.Insert(index, item);
        });

        NotifyItemAdded(item, index);
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        var removedItem = _lockingMechanism.Write(() =>
        {
            Ensure.IndexIsInRange(index, _items.Count);

            CheckReentrancy();

            var item = _items[index];

            _items.RemoveAt(index);

            return item;
        });

        NotifyItemRemoved(removedItem, index);
    }

    /// <inheritdoc />
    public int Count => _lockingMechanism.Read(() => _items.Count);

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <summary>
    ///     Gets or sets the total number of elements the internal data structure can hold
    ///     without resizing.
    /// </summary>
    public int Capacity
    {
        get => _lockingMechanism.Read(() => _items.Capacity);
        set => _lockingMechanism.Write(() => _items.Capacity = value);
    }

    /// <inheritdoc />
    public T this[int index]
    {
        get => _lockingMechanism.Read(() => _items[index]);
        set
        {
            var oldValue = _lockingMechanism.Write(() =>
            {
                Ensure.IndexIsInRange(index, _items.Count);

                CheckReentrancy();

                var oldItem = _items[index];

                _items[index] = value;

                return oldItem;
            });

            NotifyItemReplaced(oldValue, value, index);
        }
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc />
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
}
