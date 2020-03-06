using System.Collections.Generic;
using CreativeCoders.Core.Threading;

namespace CreativeCoders.Core.Caching.Default
{
    public class CacheRegion<TKey, TValue>
    {
        private readonly IDictionary<TKey, CacheEntry<TKey, TValue>> _data;

        private ILockingMechanism _dataLock;

        public CacheRegion()
        {
            _data = new Dictionary<TKey, CacheEntry<TKey, TValue>>();
            
            _dataLock = new LockSlimLockingMechanism();
        }

        public bool TryGetValue(TKey key, out CacheEntry<TKey, TValue> value)
        {
            return _data.TryGetValue(key, out value);
        }

        public void TryRemove(TKey key)
        {
            _data.Remove(key);
        }

        public void Set(TKey key, CacheEntry<TKey, TValue> cacheEntry)
        {
            _data[key] = cacheEntry;
        }

        public void Clear()
        {
            _data.Clear();
        }
    }
}