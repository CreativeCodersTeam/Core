using System;

namespace CreativeCoders.Core.Caching;

[Serializable]
public class CacheEntryNotFoundException : Exception
{
    public CacheEntryNotFoundException(string key, string regionName) : 
        base($"No cache entry for key = '{key}' and region = '{regionName ?? string.Empty}' found")
    {
        Key = key;
        RegionName = regionName ?? string.Empty;
    }        

    public string Key { get; }

    public string RegionName { get; }
}