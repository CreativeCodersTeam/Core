using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CreativeCoders.Core.Caching.Default
{
    public class DictionaryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, CacheEntry<TKey, TValue>> _data;

        public DictionaryCache()
        {
            _data = new ConcurrentDictionary<TKey, CacheEntry<TKey, TValue>>();
        }

        public TValue GetOrAdd(TKey key, Func<TValue> getValue)
        {
            if (TryGet(key, out var value))
            {
                return value;
            }

            var newValue = getValue();
            AddOrUpdate(key, newValue);

            return newValue;
        }
        
        public TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy)
        {
            if (TryGet(key, out var value))
            {
                return value;
            }

            var newValue = getValue();
            AddOrUpdate(key, newValue, expirationPolicy);

            return newValue;
        }

        public Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue)
        {
            return Task.FromResult(GetOrAdd(key, getValue));
        }

        public Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy)
        {
            return Task.FromResult(GetOrAdd(key, getValue, expirationPolicy));
        }

        public bool TryGet(TKey key, out TValue value)
        {
            if (!_data.TryGetValue(key, out var cacheEntry))
            {
                value = default;
                return false;
            }

            if (cacheEntry.CheckIsExpired())
            {
                _data.TryRemove(key, out _);
                value = default;
                return false;
            }
            
            value = cacheEntry.Value;
            return true;
        }

        public Task<CacheRequestResult<TValue>> TryGetAsync(TKey key)
        {
            return Task.FromResult(
                TryGet(key, out var value)
                    ? new CacheRequestResult<TValue>(true, value)
                    : new CacheRequestResult<TValue>(false, default)
            );
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            _data[key] = new CacheEntry<TKey, TValue>(key, CacheExpirationPolicy.NeverExpire) {Value = value};
        }

        public void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy)
        {
            _data[key] = new CacheEntry<TKey, TValue>(key, expirationPolicy) {Value = value};
        }

        public Task AddOrUpdateAsync(TKey key, TValue value)
        {
            AddOrUpdate(key, value);
            
            return Task.CompletedTask;
        }

        public Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy)
        {
            AddOrUpdate(key, value, expirationPolicy);
            
            return Task.CompletedTask;
        }

        public void Clear()
        {
            _data.Clear();
        }

        public Task ClearAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        public void Remove(TKey key)
        {
            _data.TryRemove(key, out _);
        }

        public Task RemoveAsync(TKey key)
        {
            Remove(key);
            return Task.CompletedTask;
        }
    }
}