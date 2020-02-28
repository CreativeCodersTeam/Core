using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching
{
    [PublicAPI]
    public class CachedValue<TKey, TValue>
    {
        private readonly ICache<TKey, TValue> _cache;
        
        private readonly TKey _key;
        
        private readonly TValue _defaultValue;

        private readonly Func<TValue> _getValue;
        
        private readonly ICacheExpirationPolicy _expirationPolicy;

        private readonly CachedValueMode _cachedValueMode;

        public CachedValue(ICache<TKey, TValue> cache, TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy)
        {
            _cachedValueMode = CachedValueMode.GetOrAdd;
            
            _cache = cache;
            _key = key;
            _getValue = getValue;
            _expirationPolicy = expirationPolicy;
        }
        
        public CachedValue(ICache<TKey, TValue> cache, TKey key)
        {
            _cachedValueMode = CachedValueMode.GetValue;
            
            _cache = cache;
            _key = key;
        }
        
        public CachedValue(ICache<TKey, TValue> cache, TKey key, TValue defaultValue)
        {
            _cachedValueMode = CachedValueMode.GetValueOrDefault;
            
            _cache = cache;
            _key = key;
            _defaultValue = defaultValue;
        }

        private TValue GetValue()
        {
            return _cachedValueMode switch
            {
                CachedValueMode.GetOrAdd => _cache.GetOrAdd(_key, _getValue, _expirationPolicy),
                CachedValueMode.GetValue => _cache.GetValue(_key),
                CachedValueMode.GetValueOrDefault => _cache.GetValueOrDefault(_key, _defaultValue),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public TValue Value => GetValue();
    }
}