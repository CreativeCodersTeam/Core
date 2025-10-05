using System;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core.Caching.Default;

[ExcludeFromCodeCoverage]
internal class CacheEntry<TKey, TValue> : ICacheEntry<TKey, TValue>
{
    private DateTime _lastEntryCheck;

    public CacheEntry(TKey key, ICacheExpirationPolicy expirationPolicy)
    {
        Key = key;
        ExpirationPolicy = expirationPolicy;
        _lastEntryCheck = DateTime.UtcNow;
    }

    public bool CheckIsExpired()
    {
        switch (ExpirationPolicy.ExpirationMode)
        {
            case CacheExpirationMode.NeverExpire:
                return false;
            case CacheExpirationMode.AbsoluteDateTime:
                return DateTime.UtcNow > ExpirationPolicy.AbsoluteDateTime;
            case CacheExpirationMode.SlidingTimeSpan:
                var isExpired = DateTime.UtcNow > _lastEntryCheck.Add(ExpirationPolicy.SlidingTimeSpan);
                if (!isExpired)
                {
                    _lastEntryCheck = DateTime.UtcNow;
                }

                return isExpired;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public TKey Key { get; }

    public TValue Value { get; set; }

    public ICacheExpirationPolicy ExpirationPolicy { get; }
}
