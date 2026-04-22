using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Provides a default implementation of <see cref="ICacheExpirationPolicy"/> with support for
/// never-expire and absolute-date-time expiration modes.
/// </summary>
[PublicAPI]
public class CacheExpirationPolicy : ICacheExpirationPolicy
{
    /// <summary>
    /// A shared policy instance that indicates cache entries should never expire.
    /// </summary>
    public static readonly CacheExpirationPolicy NeverExpire =
        new CacheExpirationPolicy(CacheExpirationMode.NeverExpire);

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheExpirationPolicy"/> class.
    /// </summary>
    /// <param name="expirationMode">Expiration mode for this policy.</param>
    public CacheExpirationPolicy(CacheExpirationMode expirationMode)
    {
        ExpirationMode = expirationMode;
    }

    /// <summary>
    /// Creates a new expiration policy that expires at the specified absolute date and time.
    /// </summary>
    /// <param name="absoluteDateTime">Absolute date and time at which the cache entry expires. The value is converted to UTC.</param>
    /// <returns>A new <see cref="CacheExpirationPolicy"/> configured for absolute-date-time expiration.</returns>
    public static CacheExpirationPolicy AfterAbsoluteDateTime(DateTime absoluteDateTime)
    {
        return new CacheExpirationPolicy(CacheExpirationMode.AbsoluteDateTime)
        {
            AbsoluteDateTime = absoluteDateTime.ToUniversalTime()
        };
    }

    /// <inheritdoc/>
    public CacheExpirationMode ExpirationMode { get; }

    /// <inheritdoc/>
    public DateTime AbsoluteDateTime { get; private set; }
}
