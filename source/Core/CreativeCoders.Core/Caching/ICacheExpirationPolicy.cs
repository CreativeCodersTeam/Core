using System;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Defines the expiration behavior for a cache entry.
/// </summary>
public interface ICacheExpirationPolicy
{
    /// <summary>
    /// Gets the expiration mode that determines how the cache entry expires.
    /// </summary>
    CacheExpirationMode ExpirationMode { get; }

    /// <summary>
    /// Gets the absolute UTC date and time at which the cache entry expires.
    /// </summary>
    DateTime AbsoluteDateTime { get; }
}
