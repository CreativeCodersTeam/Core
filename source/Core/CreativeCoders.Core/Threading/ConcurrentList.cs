using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Threading
{
    public class ConcurrentList<T> : IList<T>, IReadOnlyCollection<T>
    {
        private readonly ILockingMechanism _locking;

        private readonly List<T> _items;

        public ConcurrentList() : this(DefaultLockingMechanism()) {}

        public ConcurrentList(ILockingMechanism lockingMechanism)
        {
            Ensure.IsNotNull(lockingMechanism, nameof(lockingMechanism));

            _locking = lockingMechanism;
            _items = new List<T>();
        }

        public ConcurrentList(IEnumerable<T> collection) : this(collection, DefaultLockingMechanism()) {}

        public ConcurrentList(IEnumerable<T> collection, ILockingMechanism lockingMechanism)
        {
            Ensure.IsNotNull(lockingMechanism, nameof(lockingMechanism));
            Ensure.IsNotNull(collection, nameof(collection));

            _locking = lockingMechanism;
            _items = new List<T>(collection);
        }

        private static ILockingMechanism DefaultLockingMechanism()
        {
            return new LockSlimLockingMechanism();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _locking.Read(() => _items.ToList().GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _locking.Write(() => _items.Add(item));
        }

        public void Clear()
        {
            _locking.Write(() => _items.Clear());
        }

        public bool Contains(T item)
        {
            return _locking.Read(() => _items.Contains(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _locking.Read(() => _items.CopyTo(array, arrayIndex));
        }

        public bool Remove(T item)
        {
            return _locking.Write(() => _items.Remove(item));
        }

        public int Count => _locking.Read(() => _items.Count);

        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return _locking.Read(() => _items.IndexOf(item));
        }

        public void Insert(int index, T item)
        {
            _locking.Write(() => _items.Insert(index, item));
        }

        public void RemoveAt(int index)
        {
            _locking.Write(() => _items.RemoveAt(index));
        }

        public T this[int index]
        {
            get { return _locking.Read(() => _items[index]); }
            set { _locking.Write(() => _items[index] = value); }
        }

        public int Capacity
        {
            get => _locking.Read(() => _items.Capacity);
            set => _locking.Write(() => _items.Capacity = value);
        }
    }
}
