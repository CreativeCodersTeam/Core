using System;
using System.Threading.Tasks;

namespace CreativeCoders.Core.Caching.Default;

public class DictionaryCache<TKey, TValue> : CacheBase<TKey, TValue>
{
    private readonly CacheRegions<TKey, TValue> _regions;
        
    public DictionaryCache()
    {
        _regions = new CacheRegions<TKey, TValue>();
    }

    public override TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy, string regionName = null)
    {
        if (TryGet(key, out var value, regionName))
        {
            return value;
        }

        var newValue = getValue();
        AddOrUpdate(key, newValue, expirationPolicy, regionName);

        return newValue;
    }

    public override Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy, string regionName = null)
    {
        return Task.FromResult(GetOrAdd(key, getValue, expirationPolicy, regionName));
    }

    public override bool TryGet(TKey key, out TValue value, string regionName = null)
    {
        if (!_regions.TryGetValue(key, out var cacheEntry, regionName))
        {
            value = default;
            return false;
        }

        if (cacheEntry.CheckIsExpired())
        {
            _regions.TryRemove(key, regionName);
            value = default;
            return false;
        }
            
        value = cacheEntry.Value;
        return true;
    }

    public override Task<CacheRequestResult<TValue>> TryGetAsync(TKey key, string regionName = null)
    {
        return Task.FromResult(
            TryGet(key, out var value, regionName)
                ? new CacheRequestResult<TValue>(true, value)
                : new CacheRequestResult<TValue>(false, default)
        );
    }

    public override void AddOrUpdate(TKey key, TValue value, string regionName = null)
    {
        _regions.Set(key, new CacheEntry<TKey, TValue>(key, CacheExpirationPolicy.NeverExpire) {Value = value}, regionName);
    }

    public override void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy, string regionName = null)
    {
        _regions.Set(key, new CacheEntry<TKey, TValue>(key, expirationPolicy) {Value = value}, regionName);
    }

    public override Task AddOrUpdateAsync(TKey key, TValue value, string regionName = null)
    {
        AddOrUpdate(key, value, regionName);
            
        return Task.CompletedTask;
    }

    public override Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy, string regionName = null)
    {
        AddOrUpdate(key, value, expirationPolicy, regionName);
            
        return Task.CompletedTask;
    }

    public override void Clear(string regionName = null)
    {
        _regions.Clear(regionName);
    }

    public override Task ClearAsync(string regionName = null)
    {
        Clear(regionName);
        return Task.CompletedTask;
    }

    public override void Remove(TKey key, string regionName = null)
    {
        _regions.TryRemove(key, regionName);
    }

    public override Task RemoveAsync(TKey key, string regionName = null)
    {
        Remove(key, regionName);
        return Task.CompletedTask;
    }
}