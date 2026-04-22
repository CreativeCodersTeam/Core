using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Provides a lazily resolved cached value backed by an <see cref="ICache{TKey, TValue}"/>. The retrieval
/// strategy is determined by the <see cref="CachedValueMode"/> selected at construction time.
/// </summary>
/// <typeparam name="TKey">The type of the cache key.</typeparam>
/// <typeparam name="TValue">The type of the cached value.</typeparam>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedValue{TKey, TValue}"/> class that uses the
    /// <see cref="CachedValueMode.GetOrAdd"/> strategy with a factory delegate and expiration policy.
    /// </summary>
    /// <param name="cache">Underlying cache to retrieve or store values in.</param>
    /// <param name="key">Cache key used for lookup.</param>
    /// <param name="getValue">Factory delegate invoked to produce the value when the key is not found.</param>
    /// <param name="expirationPolicy">Expiration policy applied to newly added entries.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    public CachedValue(ICache<TKey, TValue> cache, TKey key, Func<TValue> getValue,
        ICacheExpirationPolicy expirationPolicy, string regionName = null)
    {
        _cachedValueMode = CachedValueMode.GetOrAdd;

        _cache = cache;
        _key = key;
        _getValue = getValue;
        _expirationPolicy = expirationPolicy;
        _regionName = regionName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedValue{TKey, TValue}"/> class that uses the
    /// <see cref="CachedValueMode.GetValue"/> strategy, throwing an exception when the key is not found.
    /// </summary>
    /// <param name="cache">Underlying cache to retrieve values from.</param>
    /// <param name="key">Cache key used for lookup.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    public CachedValue(ICache<TKey, TValue> cache, TKey key, string regionName = null)
    {
        _cachedValueMode = CachedValueMode.GetValue;

        _cache = cache;
        _key = key;
        _regionName = regionName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedValue{TKey, TValue}"/> class that uses the
    /// <see cref="CachedValueMode.GetValueOrDefault"/> strategy, returning a default value when the key is not found.
    /// </summary>
    /// <param name="cache">Underlying cache to retrieve values from.</param>
    /// <param name="key">Cache key used for lookup.</param>
    /// <param name="defaultValue">Value returned when the key is not found in the cache.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
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

    /// <inheritdoc/>
    public Task<TValue> GetValueAsync()
    {
        return _cachedValueMode switch
        {
            CachedValueMode.GetOrAdd => _cache.GetOrAddAsync(_key, _getValue, _expirationPolicy, _regionName),
            CachedValueMode.GetValue => _cache.GetValueAsync(_key, true, _regionName),
            CachedValueMode.GetValueOrDefault => _cache.GetValueOrDefaultAsync(_key, _defaultValue,
                _regionName),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <inheritdoc/>
    public TValue Value => GetValue();
}
