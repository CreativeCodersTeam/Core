using System.Threading.Tasks;

namespace CreativeCoders.Core.Caching;

public static class CacheExtensions
{
    public static TValue GetValue<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key,
        bool throwExceptionIfKeyNotExists, string regionName = null)
    {
        if (cache.TryGet(key, out var value, regionName))
        {
            return value;
        }

        if (throwExceptionIfKeyNotExists)
        {
            throw new CacheEntryNotFoundException(key.ToString(), regionName);
        }

        return default;
    }

    public static async Task<TValue> GetValueAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key,
        bool throwExceptionIfKeyNotExists, string regionName = null)
    {
        var cacheRequestResult = await cache.TryGetAsync(key, regionName).ConfigureAwait(false);

        if (cacheRequestResult.EntryExists)
        {
            return cacheRequestResult.Value;
        }

        if (throwExceptionIfKeyNotExists)
        {
            throw new CacheEntryNotFoundException(key.ToString(), regionName);
        }

        return default;
    }

    public static TValue GetValue<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, string regionName = null)
    {
        return cache.GetValue(key, true, regionName);
    }
        
    public static Task<TValue> GetValueAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key, string regionName = null)
    {
        return cache.GetValueAsync(key, true, regionName);
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key,
        TValue defaultValue, string regionName = null)
    {
        return cache.TryGet(key, out var value, regionName) ? value : defaultValue;
    }

    public static async Task<TValue> GetValueOrDefaultAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key,
        TValue defaultValue, string regionName = null)
    {
        var cacheRequestResult = await cache.TryGetAsync(key, regionName).ConfigureAwait(false);

        return cacheRequestResult.EntryExists ? cacheRequestResult.Value : defaultValue;
    }
}