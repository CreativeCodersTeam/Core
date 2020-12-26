using System;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Caching;
using JetBrains.Annotations;

namespace CreativeCoders.Caching.SystemRuntimeCaching
{
    [PublicAPI]
    public class SystemRuntimeCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly ObjectCache _cache;

        public SystemRuntimeCache() : this(MemoryCache.Default)
        {
        }
        
        private SystemRuntimeCache(ObjectCache cache)
        {
            _cache = cache;
        }

        public static SystemRuntimeCache<TKey, TValue> UsingCache(ObjectCache cache) =>
            new(cache);
        
        public TValue GetOrAdd(TKey key, Func<TValue> getValue, string regionName = null)
        {
            return GetOrAddInternal(key, getValue, new CacheItemPolicy(), regionName);
        }

        public TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy, string regionName = null)
        {
            return GetOrAddInternal(key, getValue, CreateCacheItemPolicy(expirationPolicy), regionName);
        }
        
        private TValue GetOrAddInternal(TKey key, Func<TValue> getValue, CacheItemPolicy cacheItemPolicy, string regionName = null)
        {
            var cacheItem = _cache.GetCacheItem(KeyToString(key, regionName));

            if (cacheItem != null)
            {
                return (TValue) cacheItem.Value;
            }
            
            var addedValue = getValue();
            var newValue = _cache.AddOrGetExisting(KeyToString(key, regionName), addedValue, cacheItemPolicy);
            return (TValue) newValue ?? addedValue;
        }

        public Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, string regionName = null)
        {
            return GetOrAddAsyncInternal(key, getValue, new CacheItemPolicy(), regionName);
        }

        public Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy, string regionName = null)
        {
            return GetOrAddAsyncInternal(key, getValue, CreateCacheItemPolicy(expirationPolicy), regionName);
        }
        
        private Task<TValue> GetOrAddAsyncInternal(TKey key, Func<TValue> getValue, CacheItemPolicy cacheItemPolicy, string regionName = null)
        {
            return Task.FromResult(GetOrAddInternal(key, getValue, cacheItemPolicy, regionName));
        }

        public bool TryGet(TKey key, out TValue value, string regionName = null)
        {
            var cacheItem = _cache.GetCacheItem(KeyToString(key, regionName));

            if (cacheItem != null)
            {
                value = (TValue )cacheItem.Value;
                return true;
            }

            value = default;
            return false;
        }

        public Task<CacheRequestResult<TValue>> TryGetAsync(TKey key, string regionName = null)
        {
            var entryExists = TryGet(key, out var value, regionName);
            
            return Task.FromResult(new CacheRequestResult<TValue>(entryExists, value));
        }

        public void AddOrUpdate(TKey key, TValue value, string regionName = null)
        {
            AddOrUpdateInternal(key, value, new CacheItemPolicy(), regionName);
        }

        public void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy, string regionName = null)
        {
            AddOrUpdateInternal(key, value, CreateCacheItemPolicy(expirationPolicy), regionName);
        }
        
        private void AddOrUpdateInternal(TKey key, TValue value, CacheItemPolicy cacheItemPolicy, string regionName = null)
        {
            _cache.Set(KeyToString(key, regionName), value, cacheItemPolicy);
        }

        public Task AddOrUpdateAsync(TKey key, TValue value, string regionName = null)
        {
            AddOrUpdate(key, value, regionName);
            return Task.CompletedTask;
        }

        public Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy, string regionName = null)
        {
            AddOrUpdate(key, value, expirationPolicy, regionName);
            return Task.CompletedTask;
        }

        public void Clear(string regionName = null)
        {
            if (string.IsNullOrEmpty(regionName))
            {
                _cache.ForEach(entry => _cache.Remove(entry.Key));
                return;
            }
            
            _cache
                .Where(entry => entry.Key.StartsWith(regionName + "_", StringComparison.Ordinal))
                .ForEach(entry => _cache.Remove(entry.Key));
        }

        public Task ClearAsync(string regionName = null)
        {
            Clear(regionName);
            return Task.CompletedTask;
        }

        public void Remove(TKey key, string regionName = null)
        {
            _cache.Remove(KeyToString(key, regionName));
        }

        public Task RemoveAsync(TKey key, string regionName = null)
        {
            Remove(key, regionName);
            return Task.CompletedTask;
        }

        private static string KeyToString(TKey key, string regionName = null)
        {
            if (string.IsNullOrEmpty(regionName))
            {
                return key?.ToString();
            }
            return regionName + "_" + key;
        }

        private static CacheItemPolicy CreateCacheItemPolicy(ICacheExpirationPolicy expirationPolicy)
        {
            return expirationPolicy.ExpirationMode switch
            {
                CacheExpirationMode.NeverExpire => new CacheItemPolicy(),
                CacheExpirationMode.SlidingTimeSpan => new CacheItemPolicy
                {
                    SlidingExpiration = expirationPolicy.SlidingTimeSpan
                },
                CacheExpirationMode.AbsoluteDateTime => new CacheItemPolicy
                {
                    AbsoluteExpiration = expirationPolicy.AbsoluteDateTime
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
