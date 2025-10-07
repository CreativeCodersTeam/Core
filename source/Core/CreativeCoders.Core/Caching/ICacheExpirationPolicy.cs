using System;

namespace CreativeCoders.Core.Caching;

public interface ICacheExpirationPolicy
{
    CacheExpirationMode ExpirationMode { get; }

    DateTime AbsoluteDateTime { get; }
}
