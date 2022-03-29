using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CreativeCoders.Core.Caching.Default;

internal class CacheRegions<TKey, TValue>
{
    private readonly IDictionary<string, CacheRegion<TKey, TValue>> _regions;

    private readonly CacheRegion<TKey, TValue> _defaultRegion;

    public CacheRegions()
    {
        _regions = new ConcurrentDictionary<string, CacheRegion<TKey, TValue>>();

        _defaultRegion = new CacheRegion<TKey, TValue>();
    }

    private CacheRegion<TKey, TValue> GetRegion(string regionName)
    {
        if (regionName == null)
        {
            return _defaultRegion;
        }

        if (_regions.TryGetValue(regionName, out var region))
        {
            return region;
        }

        var newRegion = new CacheRegion<TKey, TValue>();
        _regions[regionName] = newRegion;

        return newRegion;
    }

    public bool TryGetValue(TKey key, out CacheEntry<TKey, TValue> value, string regionName)
    {
        return GetRegion(regionName).TryGetValue(key, out value);
    }

    public void TryRemove(TKey key, string regionName)
    {
        GetRegion(regionName).TryRemove(key);
    }

    public void Set(TKey key, CacheEntry<TKey, TValue> cacheEntry, string regionName)
    {
        GetRegion(regionName).Set(key, cacheEntry);
    }

    public void Clear(string regionName)
    {
        GetRegion(regionName).Clear();
    }
}