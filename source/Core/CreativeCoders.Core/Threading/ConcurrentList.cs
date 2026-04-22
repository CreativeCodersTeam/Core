using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides a thread-safe implementation of <see cref="IList{T}"/> that uses
///     an <see cref="ILockingMechanism"/> to synchronize access to the underlying list.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
[PublicAPI]
public class ConcurrentList<T> : IList<T>, IReadOnlyCollection<T>
{
    private readonly List<T> _items;
    private readonly ILockingMechanism _locking;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConcurrentList{T}"/> class
    ///     with a default <see cref="LockSlimLockingMechanism"/>.
    /// </summary>
    public ConcurrentList() : this(DefaultLockingMechanism()) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConcurrentList{T}"/> class
    ///     with the specified locking mechanism.
    /// </summary>
    /// <param name="lockingMechanism">The locking mechanism used to synchronize access.</param>
    public ConcurrentList(ILockingMechanism lockingMechanism)
    {
        Ensure.IsNotNull(lockingMechanism);

        _locking = lockingMechanism;
        _items = [];
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConcurrentList{T}"/> class
    ///     with elements copied from the specified collection and a default locking mechanism.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    public ConcurrentList(IEnumerable<T> collection) : this(collection, DefaultLockingMechanism()) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConcurrentList{T}"/> class
    ///     with elements copied from the specified collection and the specified locking mechanism.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    /// <param name="lockingMechanism">The locking mechanism used to synchronize access.</param>
    public ConcurrentList(IEnumerable<T> collection, ILockingMechanism lockingMechanism)
    {
        Ensure.IsNotNull(lockingMechanism, nameof(lockingMechanism));
        Ensure.IsNotNull(collection, nameof(collection));

        _locking = lockingMechanism;
        _items = [..collection];
    }

    private static LockSlimLockingMechanism DefaultLockingMechanism()
    {
        return new LockSlimLockingMechanism();
    }

    /// <inheritdoc />
    [MustDisposeResource]
    public IEnumerator<T> GetEnumerator()
        => _locking.Read([MustDisposeResource]() => _items.ToList().GetEnumerator());

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public void Add(T item)
    {
        _locking.Write(() => _items.Add(item));
    }

    /// <inheritdoc />
    public void Clear()
    {
        _locking.Write(() => _items.Clear());
    }

    /// <inheritdoc />
    public bool Contains(T item)
    {
        return _locking.Read(() => _items.Contains(item));
    }

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex)
    {
        _locking.Read(() => _items.CopyTo(array, arrayIndex));
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
        return _locking.Write(() => _items.Remove(item));
    }

    /// <inheritdoc />
    public int IndexOf(T item)
    {
        return _locking.Read(() => _items.IndexOf(item));
    }

    /// <inheritdoc />
    public void Insert(int index, T item)
    {
        _locking.Write(() => _items.Insert(index, item));
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        _locking.Write(() => _items.RemoveAt(index));
    }

    /// <inheritdoc />
    public int Count => _locking.Read(() => _items.Count);

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public T this[int index]
    {
        get { return _locking.Read(() => _items[index]); }
        set { _locking.Write(() => _items[index] = value); }
    }

    /// <summary>
    ///     Gets or sets the total number of elements the internal data structure can hold
    ///     without resizing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public int Capacity
    {
        get => _locking.Read(() => _items.Capacity);
        set => _locking.Write(() => _items.Capacity = value);
    }
}
