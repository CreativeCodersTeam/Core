namespace CreativeCoders.Core.Caching;

/// <summary>
/// Specifies the retrieval strategy used by a <see cref="CachedValue{TKey, TValue}"/> instance.
/// </summary>
public enum CachedValueMode
{
    /// <summary>
    /// Gets the cached value or adds it using a factory delegate if the key does not exist.
    /// </summary>
    GetOrAdd,

    /// <summary>
    /// Gets the cached value, throwing an exception if the key does not exist.
    /// </summary>
    GetValue,

    /// <summary>
    /// Gets the cached value, returning a default value if the key does not exist.
    /// </summary>
    GetValueOrDefault
}
