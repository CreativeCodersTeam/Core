using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

[PublicAPI]
public interface ICache<in TKey, TValue>
{
    TValue GetOrAdd(TKey key, Func<TValue> getValue, string regionName = null);

    TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, string regionName = null);

    Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    bool TryGet(TKey key, out TValue value, string regionName = null);

    Task<CacheRequestResult<TValue>> TryGetAsync(TKey key, string regionName = null);

    void AddOrUpdate(TKey key, TValue value, string regionName = null);

    void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    Task AddOrUpdateAsync(TKey key, TValue value, string regionName = null);

    Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    void Clear(string regionName = null);

    Task ClearAsync(string regionName = null);

    void Remove(TKey key, string regionName = null);

    Task RemoveAsync(TKey key, string regionName = null);
}
