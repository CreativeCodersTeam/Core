using System.Threading.Tasks;

namespace CreativeCoders.Core.Caching
{
    public static class CacheExtensions
    {
        // public static TValue GetOrAdd<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, Func<TValue> getValue)
        // {
        //     return cache.GetOrAdd(key, getValue, CacheExpirationPolicy.NeverExpire);
        // }
        
        // public static Task<TValue> GetOrAddAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, Func<TValue> getValue)
        // {
        //     return cache.GetOrAddAsync(key, getValue, CacheExpirationPolicy.NeverExpire);
        // }

        // public static void AddOrUpdate<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value)
        // {
        //     cache.AddOrUpdate(key, value, CacheExpirationPolicy.NeverExpire);
        // }
        //
        // public static Task AddOrUpdateAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue value)
        // {
        //     return cache.AddOrUpdateAsync(key, value, CacheExpirationPolicy.NeverExpire);
        // }

        public static TValue GetValue<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, bool throwExceptionIfKeyNotExists)
        {
            if (cache.TryGet(key, out var value))
            {
                return value;
            }

            if (throwExceptionIfKeyNotExists)
            {
                throw new CacheEntryNotFoundException(key.ToString());
            }

            return default;
        }
        
        public static async Task<TValue> GetValueAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, bool throwExceptionIfKeyNotExists)
        {
            var cacheRequestResult = await cache.TryGetAsync(key).ConfigureAwait(false);
            
            if (cacheRequestResult.EntryExists)
            {
                return cacheRequestResult.Value;
            }

            if (throwExceptionIfKeyNotExists)
            {
                throw new CacheEntryNotFoundException(key.ToString());
            }

            return default;
        }
        
        public static TValue GetValue<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key)
        {
            return cache.GetValue(key, true);
        }
        
        public static Task<TValue> GetValueAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key)
        {
            return cache.GetValueAsync(key, true);
        }
        
        public static TValue GetValueOrDefault<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue defaultValue)
        {
            return cache.TryGet(key, out var value) ? value : defaultValue;
        }
        
        public static async Task<TValue> GetValueOrDefaultAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, TValue defaultValue)
        {
            var cacheRequestResult = await cache.TryGetAsync(key).ConfigureAwait(false);
            
            return cacheRequestResult.EntryExists ? cacheRequestResult.Value : defaultValue;
        }
    }
}