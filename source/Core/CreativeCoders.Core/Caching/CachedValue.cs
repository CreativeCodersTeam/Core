using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

[PublicAPI]
public class CachedValue<TKey, TValue> : ICachedValue<TValue>
{
    private readonly ICache<TKey, TValue> _cache;
        
    private readonly TKey _key;
        
    private readonly TValue _defaultValue;

    private readonly Func<TValue> _getValue;
        
    private readonly ICacheExpirationPolicy _expirationPolicy;
        
    private readonly string _regionName;

    private readonly CachedValueMode _cachedValueMode;

    public CachedValue(ICache<TKey, TValue> cache, TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy, string regionName = null)
    {
        _cachedValueMode = CachedValueMode.GetOrAdd;
            
        _cache = cache;
        _key = key;
        _getValue = getValue;
        _expirationPolicy = expirationPolicy;
        _regionName = regionName;
    }
        
    public CachedValue(ICache<TKey, TValue> cache, TKey key, string regionName = null)
    {
        _cachedValueMode = CachedValueMode.GetValue;
            
        _cache = cache;
        _key = key;
        _regionName = regionName;
    }
        
    public CachedValue(ICache<TKey, TValue> cache, TKey key, TValue defaultValue, string regionName = null)
    {
        _cachedValueMode = CachedValueMode.GetValueOrDefault;
            
        _cache = cache;
        _key = key;
        _defaultValue = defaultValue;
        _regionName = regionName;
    }

    private TValue GetValue()
    {
        return _cachedValueMode switch
        {
            CachedValueMode.GetOrAdd => _cache.GetOrAdd(_key, _getValue, _expirationPolicy, _regionName),
            CachedValueMode.GetValue => _cache.GetValue(_key, true, _regionName),
            CachedValueMode.GetValueOrDefault => _cache.GetValueOrDefault(_key, _defaultValue, _regionName),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public Task<TValue> GetValueAsync()
    {
        return _cachedValueMode switch
        {
            CachedValueMode.GetOrAdd => _cache.GetOrAddAsync(_key, _getValue, _expirationPolicy, _regionName),
            CachedValueMode.GetValue => _cache.GetValueAsync(_key, true, _regionName),
            CachedValueMode.GetValueOrDefault => _cache.GetValueOrDefaultAsync(_key, _defaultValue, _regionName),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public TValue Value => GetValue();
}