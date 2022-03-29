using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

[PublicAPI]
public interface ICacheEntry<out TKey, TValue>
{
    TKey Key { get; }
        
    TValue Value { get; set; }
        
    ICacheExpirationPolicy ExpirationPolicy { get; }
}