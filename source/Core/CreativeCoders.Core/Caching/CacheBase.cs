using System;

namespace CreativeCoders.Core.Caching
{
    public abstract class CacheBase<TKey, TValue> : ICache<TKey, TValue>
    {
        public TValue GetValue(TKey key)
        {
            return GetValue(key, true);
        }

        public TValue GetValue(TKey key, bool throwExceptionIfKeyNotExists)
        {
            if (TryGetValue(key, out var value))
            {
                return value;
            }

            if (!throwExceptionIfKeyNotExists)
            {
                return default;
            }

            throw new CacheEntryNotFoundException(key?.ToString());
        }

        public TValue GetValue(TKey key, TValue defaultValue)
        {
            return TryGetValue(key, out var value) ? value : defaultValue;
        }

        public TValue GetValue(TKey key, Func<TValue> addValueFunc)
        {
            Ensure.IsNotNull(addValueFunc, nameof(addValueFunc));

            if (TryGetValue(key, out var value))
            {
                return value;
            }

            var newValue = addValueFunc();
            AddOrUpdate(key, newValue);

            return newValue;
        }

        public abstract bool TryGetValue(TKey key, out TValue value);

        public abstract void AddOrUpdate(TKey key, TValue value);

        public abstract void Clear();
    }
}