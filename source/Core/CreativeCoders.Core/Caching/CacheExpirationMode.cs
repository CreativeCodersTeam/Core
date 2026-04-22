namespace CreativeCoders.Core.Caching;

/// <summary>
/// Specifies the mode used to determine when a cache entry expires.
/// </summary>
public enum CacheExpirationMode
{
    /// <summary>
    /// The cache entry never expires.
    /// </summary>
    NeverExpire,

    /// <summary>
    /// The cache entry expires at a specific absolute date and time.
    /// </summary>
    AbsoluteDateTime
}
