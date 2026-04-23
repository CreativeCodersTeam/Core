using System;
using System.Threading.Tasks;

namespace CreativeCoders.Core.Caching.Default;

/// <summary>
/// Provides a dictionary-based in-memory implementation of <see cref="CacheBase{TKey, TValue}"/>.
/// </summary>
/// <typeparam name="TKey">The type of the cache key.</typeparam>
/// <typeparam name="TValue">The type of the cached value.</typeparam>
public class DictionaryCache<TKey, TValue> : CacheBase<TKey, TValue>
{
    private readonly CacheRegions<TKey, TValue> _regions = new CacheRegions<TKey, TValue>();

    /// <inheritdoc/>
    public override TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy,
        string regionName = null)
    {
        if (TryGet(key, out var value, regionName))
        {
            return value;
        }

        var newValue = getValue();
        AddOrUpdate(key, newValue, expirationPolicy, regionName);

        return newValue;
    }

    /// <inheritdoc/>
    public override Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue,
        ICacheExpirationPolicy expirationPolicy, string regionName = null)
    {
        return Task.FromResult(GetOrAdd(key, getValue, expirationPolicy, regionName));
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override Task<CacheRequestResult<TValue>> TryGetAsync(TKey key, string regionName = null)
    {
        return Task.FromResult(
            TryGet(key, out var value, regionName)
                ? new CacheRequestResult<TValue>(true, value)
                : new CacheRequestResult<TValue>(false, default)
        );
    }

    /// <inheritdoc/>
    public override void AddOrUpdate(TKey key, TValue value, string regionName = null)
    {
        _regions.Set(key,
            new CacheEntry<TKey, TValue>(key, CacheExpirationPolicy.NeverExpire) { Value = value },
            regionName);
    }

    /// <inheritdoc/>
    public override void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy,
        string regionName = null)
    {
        _regions.Set(key, new CacheEntry<TKey, TValue>(key, expirationPolicy) { Value = value }, regionName);
    }

    /// <inheritdoc/>
    public override Task AddOrUpdateAsync(TKey key, TValue value, string regionName = null)
    {
        AddOrUpdate(key, value, regionName);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy,
        string regionName = null)
    {
        AddOrUpdate(key, value, expirationPolicy, regionName);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override void Clear(string regionName = null)
    {
        _regions.Clear(regionName);
    }

    /// <inheritdoc/>
    public override Task ClearAsync(string regionName = null)
    {
        Clear(regionName);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override void Remove(TKey key, string regionName = null)
    {
        _regions.TryRemove(key, regionName);
    }

    /// <inheritdoc/>
    public override Task RemoveAsync(TKey key, string regionName = null)
    {
        Remove(key, regionName);
        return Task.CompletedTask;
    }
}
