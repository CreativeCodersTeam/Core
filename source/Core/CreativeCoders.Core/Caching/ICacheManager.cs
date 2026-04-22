using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Provides access to named cache instances.
/// </summary>
[PublicAPI]
public interface ICacheManager
{
    /// <summary>
    /// Gets or creates a cache instance with the specified name.
    /// </summary>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <typeparam name="TValue">The type of the cached value.</typeparam>
    /// <param name="name">Unique name identifying the cache.</param>
    /// <returns>The <see cref="ICache{TKey, TValue}"/> instance associated with the given name.</returns>
    ICache<TKey, TValue> GetCache<TKey, TValue>(string name);
}
