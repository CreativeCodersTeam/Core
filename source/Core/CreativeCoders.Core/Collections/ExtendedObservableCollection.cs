using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CreativeCoders.Core.Threading;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Collections;

[PublicAPI]
public class ExtendedObservableCollection<T> : IList<T>, IReadOnlyList<T>, INotifyPropertyChanged,
    INotifyCollectionChanged
{
    private const string IndexerName = "Item[]";

    private readonly SynchronizationContext? _synchronizationContext;

    private readonly SynchronizationMethod _synchronizationMethod;

    private readonly ILockingMechanism _lockingMechanism;

    private readonly List<T> _items;

    private readonly SynchronizedValue<int> _updateCounter;

    private readonly SynchronizedValue<bool> _collectionHasChanged;

    private readonly SimpleMonitor _reentrancyMonitor;

    public ExtendedObservableCollection()
        : this(SynchronizationContext.Current, SynchronizationMethod.Send,
            () => new LockSlimLockingMechanism(), Array.Empty<T>()) { }

    public ExtendedObservableCollection(IEnumerable<T> items)
        : this(SynchronizationContext.Current, SynchronizationMethod.Send,
            () => new LockSlimLockingMechanism(), items) { }

    public ExtendedObservableCollection(SynchronizationContext? synchronizationContext,
        SynchronizationMethod synchronizationMethod, Func<ILockingMechanism> lockingMechanism)
        : this(synchronizationContext, synchronizationMethod, lockingMechanism, Array.Empty<T>()) { }

    public ExtendedObservableCollection(SynchronizationContext? synchronizationContext,
        SynchronizationMethod synchronizationMethod, Func<ILockingMechanism> lockingMechanism,
        IEnumerable<T> items)
    {
        Ensure.IsNotNull(synchronizationMethod, nameof(synchronizationMethod));
        Ensure.IsNotNull(lockingMechanism, nameof(lockingMechanism));
        Ensure.IsNotNull(items, nameof(items));

        _synchronizationContext = synchronizationContext;
        _synchronizationMethod =
            _synchronizationContext != null ? synchronizationMethod : SynchronizationMethod.None;
        _lockingMechanism = lockingMechanism();

        _items = new List<T>(items);
        _updateCounter = SynchronizedValue.Create<int>(lockingMechanism());
        _collectionHasChanged = SynchronizedValue.Create<bool>(lockingMechanism());
        _reentrancyMonitor = new SimpleMonitor();
    }

    [MustDisposeResource]
    public IEnumerator<T> GetEnumerator()
        => _lockingMechanism.Read([MustDisposeResource]() => _items.ToList().GetEnumerator());

    [MustDisposeResource]
    IEnumerator IEnumerable.GetEnumerator()
        => _lockingMechanism.Read([MustDisposeResource]() => _items.ToList().GetEnumerator());

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

    public void AddRange(IEnumerable<T> items)
    {
        _lockingMechanism.Write(() =>
        {
            CheckReentrancy();

            _items.AddRange(items);
        });

        NotifyItemsReset();
    }

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

    public bool Contains(T item) => _lockingMechanism.Read(() => _items.Contains(item));

    public void CopyTo(T[] array, int arrayIndex)
    {
        _lockingMechanism.Read(() => Array.Copy(_items.ToArray(), 0, array, arrayIndex, _items.Count));
    }

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

    public int Count => _lockingMechanism.Read(() => _items.Count);

    public bool IsReadOnly => false;

    public int IndexOf(T item) => _lockingMechanism.Read(() => _items.IndexOf(item));

    public void Insert(int index, T item)
    {
        _lockingMechanism.Write(() =>
        {
            CheckReentrancy();

            _items.Insert(index, item);
        });

        NotifyItemAdded(item, index);
    }

    public void RemoveAt(int index)
    {
        var removedItem = _lockingMechanism.Write(() =>
        {
            Ensure.IndexIsInRange(index, _items.Count, nameof(index));

            CheckReentrancy();

            var item = _items[index];

            _items.RemoveAt(index);

            return item;
        });

        NotifyItemRemoved(removedItem, index);
    }

    public void Move(int oldIndex, int newIndex)
    {
        var movedItem = _lockingMechanism.Write(() =>
        {
            Ensure.IndexIsInRange(oldIndex, _items.Count, nameof(oldIndex));
            Ensure.IndexIsInRange(newIndex, _items.Count, nameof(newIndex));

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

    public int Capacity
    {
        get => _lockingMechanism.Read(() => _items.Capacity);
        set => _lockingMechanism.Write(() => _items.Capacity = value);
    }

    public T this[int index]
    {
        get => _lockingMechanism.Read(() => _items[index]);
        set
        {
            var oldValue = _lockingMechanism.Write(() =>
            {
                Ensure.IndexIsInRange(index, _items.Count, nameof(index));

                CheckReentrancy();

                var oldItem = _items[index];

                _items[index] = value;

                return oldItem;
            });

            NotifyItemReplaced(oldValue, value, index);
        }
    }

    public IDisposable Update()
    {
        BeginUpdate();

        return new DelegateDisposable(EndUpdate, true);
    }

    public void BeginUpdate() => _updateCounter.SetValue(x => ++x);

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

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (_updateCounter.Value > 0)
        {
            _collectionHasChanged.Value = true;
            return;
        }

        CollectionChanged?.Invoke(this, e);
    }
}
