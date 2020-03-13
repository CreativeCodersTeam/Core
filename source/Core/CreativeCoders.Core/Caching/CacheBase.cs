using System;
using System.Threading.Tasks;

namespace CreativeCoders.Core.Caching
{
    public abstract class CacheBase<TKey, TValue> : ICache<TKey, TValue>
    {
        public virtual TValue GetOrAdd(TKey key, Func<TValue> getValue, string regionName = null)
        {
            return GetOrAdd(key, getValue, CacheExpirationPolicy.NeverExpire, regionName);
        }
        
        public abstract TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy, string regionName = null);

        public virtual Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, string regionName = null)
        {
            return GetOrAddAsync(key, getValue, CacheExpirationPolicy.NeverExpire, regionName);
        }

        public abstract Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue,
            ICacheExpirationPolicy expirationPolicy, string regionName = null);

        public abstract bool TryGet(TKey key, out TValue value, string regionName = null);

        public abstract Task<CacheRequestResult<TValue>> TryGetAsync(TKey key, string regionName = null);
        
        public abstract void AddOrUpdate(TKey key, TValue value, string regionName = null);

        public abstract void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy, string regionName = null);
        
        public abstract Task AddOrUpdateAsync(TKey key, TValue value, string regionName = null);

        public abstract Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy, string regionName = null);

        public abstract void Clear(string regionName = null);

        public abstract Task ClearAsync(string regionName = null);

        public abstract void Remove(TKey key, string regionName = null);

        public abstract Task RemoveAsync(TKey key, string regionName = null);
    }
}