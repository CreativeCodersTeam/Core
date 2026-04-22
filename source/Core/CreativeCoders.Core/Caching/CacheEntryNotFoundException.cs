using System;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// The exception that is thrown when a cache entry with the specified key and region is not found.
/// </summary>
[Serializable]
public class CacheEntryNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheEntryNotFoundException"/> class.
    /// </summary>
    /// <param name="key">Cache key that was not found.</param>
    /// <param name="regionName">Cache region name that was searched, or <see langword="null"/> for the default region.</param>
    public CacheEntryNotFoundException(string key, string regionName) :
        base($"No cache entry for key = '{key}' and region = '{regionName ?? string.Empty}' found")
    {
        Key = key;
        RegionName = regionName ?? string.Empty;
    }

    /// <summary>
    /// Gets the cache key that was not found.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the name of the cache region that was searched.
    /// </summary>
    public string RegionName { get; }
}
