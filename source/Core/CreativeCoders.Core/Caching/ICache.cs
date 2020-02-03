using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching
{
    [PublicAPI]
    public interface ICache<in TKey, TValue>
    {
        TValue GetValue(TKey key);

        TValue GetValue(TKey key, bool throwExceptionIfKeyNotExists);

        TValue GetValue(TKey key, TValue defaultValue);

        TValue GetValue(TKey key, Func<TValue> addValueFunc);

        bool TryGetValue(TKey key, out TValue value);

        void AddOrUpdate(TKey key, TValue value);

        void Clear();
    }
}