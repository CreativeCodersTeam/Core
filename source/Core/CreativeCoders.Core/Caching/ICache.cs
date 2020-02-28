using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching
{
    [PublicAPI]
    public interface ICache<TKey, TValue>
    {
        TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy);
        
        Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy);

        bool TryGet(TKey key, out TValue value);
        
        Task<CacheRequestResult<TValue>> TryGetAsync(TKey key);

        void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy);
        
        Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy);

        ICacheEntry<TKey, TValue> GetEntry(TKey key);
        
        Task<ICacheEntry<TKey, TValue>> GetEntryAsync(TKey key);

        void Clear();
        
        Task ClearAsync();

        void Remove(TKey key);
        
        Task RemoveAsync(TKey key);
    }
}