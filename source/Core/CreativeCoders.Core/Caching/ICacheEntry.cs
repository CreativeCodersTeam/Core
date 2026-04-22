using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Represents a single entry stored in a cache, including its key, value, and expiration policy.
/// </summary>
/// <typeparam name="TKey">The type of the cache key.</typeparam>
/// <typeparam name="TValue">The type of the cached value.</typeparam>
[PublicAPI]
public interface ICacheEntry<out TKey, TValue>
{
    /// <summary>
    /// Gets the key that identifies this cache entry.
    /// </summary>
    TKey Key { get; }

    /// <summary>
    /// Gets or sets the value stored in this cache entry.
    /// </summary>
    TValue Value { get; set; }

    /// <summary>
    /// Gets the expiration policy governing when this cache entry expires.
    /// </summary>
    ICacheExpirationPolicy ExpirationPolicy { get; }
}
