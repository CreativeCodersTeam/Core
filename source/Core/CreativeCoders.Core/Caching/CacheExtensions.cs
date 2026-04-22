using System.Threading.Tasks;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Provides extension methods for <see cref="ICache{TKey, TValue}"/> that simplify common value retrieval patterns.
/// </summary>
public static class CacheExtensions
{
    /// <summary>
    /// Gets the value associated with the specified key, optionally throwing an exception when the key is not found.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cached value.</typeparam>
    /// <param name="cache">Cache to retrieve the value from.</param>
    /// <param name="key">Cache key to look up.</param>
    /// <param name="throwExceptionIfKeyNotExists">When <see langword="true"/>, a <see cref="CacheEntryNotFoundException"/> is thrown if the key is not found; otherwise, the default value is returned.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>The cached value, or the default value when <paramref name="throwExceptionIfKeyNotExists"/> is <see langword="false"/> and the key is not found.</returns>
    /// <exception cref="CacheEntryNotFoundException">The <paramref name="key"/> does not exist and <paramref name="throwExceptionIfKeyNotExists"/> is <see langword="true"/>.</exception>
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

    /// <summary>
    /// Asynchronously gets the value associated with the specified key, optionally throwing an exception when the key is not found.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cached value.</typeparam>
    /// <param name="cache">Cache to retrieve the value from.</param>
    /// <param name="key">Cache key to look up.</param>
    /// <param name="throwExceptionIfKeyNotExists">When <see langword="true"/>, a <see cref="CacheEntryNotFoundException"/> is thrown if the key is not found; otherwise, the default value is returned.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached value, or the default value when <paramref name="throwExceptionIfKeyNotExists"/> is <see langword="false"/> and the key is not found.</returns>
    /// <exception cref="CacheEntryNotFoundException">The <paramref name="key"/> does not exist and <paramref name="throwExceptionIfKeyNotExists"/> is <see langword="true"/>.</exception>
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

    /// <summary>
    /// Gets the value associated with the specified key, throwing a <see cref="CacheEntryNotFoundException"/> when the key is not found.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cached value.</typeparam>
    /// <param name="cache">Cache to retrieve the value from.</param>
    /// <param name="key">Cache key to look up.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>The cached value.</returns>
    /// <exception cref="CacheEntryNotFoundException">The <paramref name="key"/> does not exist in the cache.</exception>
    public static TValue GetValue<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key,
        string regionName = null)
    {
        return cache.GetValue(key, true, regionName);
    }

    /// <summary>
    /// Asynchronously gets the value associated with the specified key, throwing a <see cref="CacheEntryNotFoundException"/> when the key is not found.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cached value.</typeparam>
    /// <param name="cache">Cache to retrieve the value from.</param>
    /// <param name="key">Cache key to look up.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached value.</returns>
    /// <exception cref="CacheEntryNotFoundException">The <paramref name="key"/> does not exist in the cache.</exception>
    public static Task<TValue> GetValueAsync<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key,
        string regionName = null)
    {
        return cache.GetValueAsync(key, true, regionName);
    }

    /// <summary>
    /// Gets the value associated with the specified key, or returns a default value when the key is not found.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cached value.</typeparam>
    /// <param name="cache">Cache to retrieve the value from.</param>
    /// <param name="key">Cache key to look up.</param>
    /// <param name="defaultValue">Value to return when the key is not found.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>The cached value if the key exists; otherwise, <paramref name="defaultValue"/>.</returns>
    public static TValue GetValueOrDefault<TKey, TValue>(this ICache<TKey, TValue> cache, TKey key,
        TValue defaultValue, string regionName = null)
    {
        return cache.TryGet(key, out var value, regionName) ? value : defaultValue;
    }

    /// <summary>
    /// Asynchronously gets the value associated with the specified key, or returns a default value when the key is not found.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cached value.</typeparam>
    /// <param name="cache">Cache to retrieve the value from.</param>
    /// <param name="key">Cache key to look up.</param>
    /// <param name="defaultValue">Value to return when the key is not found.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached value if the key exists; otherwise, <paramref name="defaultValue"/>.</returns>
    public static async Task<TValue> GetValueOrDefaultAsync<TKey, TValue>(this ICache<TKey, TValue> cache,
        TKey key,
        TValue defaultValue, string regionName = null)
    {
        var cacheRequestResult = await cache.TryGetAsync(key, regionName).ConfigureAwait(false);

        return cacheRequestResult.EntryExists ? cacheRequestResult.Value : defaultValue;
    }
}
