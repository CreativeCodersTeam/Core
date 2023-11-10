using System.Collections.Generic;

namespace CreativeCoders.Core.Caching.Default;

internal class CacheRegion<TKey, TValue>
{
    private readonly Dictionary<TKey, CacheEntry<TKey, TValue>> _data = new();

    public bool TryGetValue(TKey key, out CacheEntry<TKey, TValue> value)
    {
        return _data.TryGetValue(key, out value);
    }

    public void TryRemove(TKey key)
    {
        _data.Remove(key);
    }

    public void Set(TKey key, CacheEntry<TKey, TValue> cacheEntry)
    {
        _data[key] = cacheEntry;
    }

    public void Clear()
    {
        _data.Clear();
    }
}
