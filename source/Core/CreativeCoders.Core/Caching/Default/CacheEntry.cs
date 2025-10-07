using System;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core.Caching.Default;

[ExcludeFromCodeCoverage]
internal class CacheEntry<TKey, TValue> : ICacheEntry<TKey, TValue>
{
    public CacheEntry(TKey key, ICacheExpirationPolicy expirationPolicy)
    {
        Key = key;
        ExpirationPolicy = expirationPolicy;
    }

    public bool CheckIsExpired()
    {
        return ExpirationPolicy.ExpirationMode switch
        {
            CacheExpirationMode.NeverExpire => false,
            CacheExpirationMode.AbsoluteDateTime => DateTime.UtcNow > ExpirationPolicy.AbsoluteDateTime,
            _ => throw new ArgumentOutOfRangeException(message: "Unknown expiration mode", null)
        };
    }

    public TKey Key { get; }

    public TValue Value { get; set; }

    public ICacheExpirationPolicy ExpirationPolicy { get; }
}
