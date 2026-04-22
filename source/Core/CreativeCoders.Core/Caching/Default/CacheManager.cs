namespace CreativeCoders.Core.Caching.Default;

/// <summary>
/// Provides a static factory for creating default in-memory cache instances.
/// </summary>
public static class CacheManager
{
    /// <summary>
    /// Creates a new <see cref="DictionaryCache{TKey, TValue}"/> instance.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cached value.</typeparam>
    /// <returns>A new <see cref="ICache{TKey, TValue}"/> backed by an in-memory dictionary.</returns>
    public static ICache<TKey, TValue> CreateCache<TKey, TValue>()
    {
        return new DictionaryCache<TKey, TValue>();
    }
}
