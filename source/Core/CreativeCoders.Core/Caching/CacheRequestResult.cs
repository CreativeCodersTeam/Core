namespace CreativeCoders.Core.Caching;

/// <summary>
/// Represents the result of a cache lookup, indicating whether the entry exists and its value.
/// </summary>
/// <typeparam name="TValue">The type of the cached value.</typeparam>
public class CacheRequestResult<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheRequestResult{TValue}"/> class.
    /// </summary>
    /// <param name="entryExists">Indicates whether the cache entry was found.</param>
    /// <param name="value">Cached value, or the default value if the entry does not exist.</param>
    public CacheRequestResult(bool entryExists, TValue value)
    {
        EntryExists = entryExists;
        Value = value;
    }

    /// <summary>
    /// Gets a value indicating whether the requested cache entry exists.
    /// </summary>
    public bool EntryExists { get; }

    /// <summary>
    /// Gets the cached value. The value is meaningful only when <see cref="EntryExists"/> is <see langword="true"/>.
    /// </summary>
    public TValue Value { get; }
}
