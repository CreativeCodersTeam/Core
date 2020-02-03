using System.Collections.Concurrent;

namespace CreativeCoders.Core.Caching
{
    public class DictionaryCache<TKey, TValue> : CacheBase<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _data;

        public DictionaryCache()
        {
            _data = new ConcurrentDictionary<TKey, TValue>();
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            return _data.TryGetValue(key, out value);
        }

        public override void AddOrUpdate(TKey key, TValue value)
        {
            _data[key] = value;
        }

        public override void Clear()
        {
            _data.Clear();
        }
    }
}