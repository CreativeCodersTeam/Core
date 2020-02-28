namespace CreativeCoders.Core.Caching.Default
{
    public class CacheEntry<TKey, TValue> : ICacheEntry<TKey, TValue>
    {
        public CacheEntry(TKey key, ICacheExpirationPolicy expirationPolicy)
        {
            Key = key;
            ExpirationPolicy = expirationPolicy;
        }

        public TKey Key { get; }

        public TValue Value { get; set; }

        public ICacheExpirationPolicy ExpirationPolicy { get; }
    }
}