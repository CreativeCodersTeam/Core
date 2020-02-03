namespace CreativeCoders.Core.Caching
{
    public static class CacheManager
    {
        public static ICache<TKey, TValue> CreateCache<TKey, TValue>()
        {
            return new DictionaryCache<TKey, TValue>();
        }
    }
}