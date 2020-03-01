using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching
{
    [PublicAPI]
    public interface ICache<in TKey, TValue>
    {
        //TValue GetOrAdd(TKey key, Func<TValue> getValue);
        
        TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy);
        
        //Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue);
        
        Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy);

        bool TryGet(TKey key, out TValue value);
        
        Task<CacheRequestResult<TValue>> TryGetAsync(TKey key);

        //void AddOrUpdate(TKey key, TValue value);
        
        void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy);
        
        //Task AddOrUpdateAsync(TKey key, TValue value);
        
        Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy);

        void Clear();
        
        Task ClearAsync();

        void Remove(TKey key);
        
        Task RemoveAsync(TKey key);
    }
}