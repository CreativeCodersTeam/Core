using System;
using System.Threading.Tasks;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Provides an abstract base class for <see cref="ICache{TKey, TValue}"/> implementations. Overloads without
/// an expiration policy delegate to the overloads that accept an <see cref="ICacheExpirationPolicy"/>
/// using <see cref="CacheExpirationPolicy.NeverExpire"/>.
/// </summary>
/// <typeparam name="TKey">The type of the cache key.</typeparam>
/// <typeparam name="TValue">The type of the cached value.</typeparam>
public abstract class CacheBase<TKey, TValue> : ICache<TKey, TValue>
{
    /// <inheritdoc/>
    public virtual TValue GetOrAdd(TKey key, Func<TValue> getValue, string regionName = null)
    {
        return GetOrAdd(key, getValue, CacheExpirationPolicy.NeverExpire, regionName);
    }

    /// <inheritdoc/>
    public abstract TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    /// <inheritdoc/>
    public virtual Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, string regionName = null)
    {
        return GetOrAddAsync(key, getValue, CacheExpirationPolicy.NeverExpire, regionName);
    }

    /// <inheritdoc/>
    public abstract Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue,
        ICacheExpirationPolicy expirationPolicy, string regionName = null);

    /// <inheritdoc/>
    public abstract bool TryGet(TKey key, out TValue value, string regionName = null);

    /// <inheritdoc/>
    public abstract Task<CacheRequestResult<TValue>> TryGetAsync(TKey key, string regionName = null);

    /// <inheritdoc/>
    public abstract void AddOrUpdate(TKey key, TValue value, string regionName = null);

    /// <inheritdoc/>
    public abstract void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    /// <inheritdoc/>
    public abstract Task AddOrUpdateAsync(TKey key, TValue value, string regionName = null);

    /// <inheritdoc/>
    public abstract Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    /// <inheritdoc/>
    public abstract void Clear(string regionName = null);

    /// <inheritdoc/>
    public abstract Task ClearAsync(string regionName = null);

    /// <inheritdoc/>
    public abstract void Remove(TKey key, string regionName = null);

    /// <inheritdoc/>
    public abstract Task RemoveAsync(TKey key, string regionName = null);
}
