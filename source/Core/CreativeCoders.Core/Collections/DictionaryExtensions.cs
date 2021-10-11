using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CreativeCoders.Core.Collections
{
    public static class DictionaryExtensions
    {
        public static TKey GetKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (TryGetKeyByValue(dictionary, value, out var key))
            {
                return key;
            }

            throw new KeyNotFoundException();
        }

        [SuppressMessage("ReSharper", "CompareNonConstrainedGenericWithNull")]
        public static bool TryGetKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value,
            out TKey key)
        {
            foreach (var dictEntry in dictionary.Where(dictEntry =>
                dictEntry.Value == null && value == null || dictEntry.Value?.Equals(value) == true))
            {
                key = dictEntry.Key;
                return true;
            }

            key = default;
            return false;
        }

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IDictionary dictionary, bool skipNotMatchingEntries)
        {
            var convertedDictionary = new Dictionary<TKey, TValue>();

            foreach (DictionaryEntry dictionaryEntry in dictionary)
            {
                if (!(dictionaryEntry.Key is TKey key))
                {
                    if (skipNotMatchingEntries)
                    {
                        continue;
                    }

                    throw new InvalidCastException("Base dictionary key has wrong type");
                }

                if (!(dictionaryEntry.Value is TValue value))
                {
                    if (skipNotMatchingEntries)
                    {
                        continue;
                    }

                    throw new InvalidCastException("Base dictionary value has wrong type");
                }

                convertedDictionary.Add(key, value);
            }

            return convertedDictionary;
        }

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IDictionary dictionary, Func<object, TValue> valueSelector, bool skipNotMatchingEntries)
        {
            var convertedDictionary = new Dictionary<TKey, TValue>();

            foreach (DictionaryEntry dictionaryEntry in dictionary)
            {
                if (!(dictionaryEntry.Key is TKey key))
                {
                    if (skipNotMatchingEntries)
                    {
                        continue;
                    }

                    throw new InvalidCastException("Base dictionary key has wrong type");
                }

                var value = valueSelector(dictionaryEntry.Value);

                convertedDictionary.Add(key, value);
            }

            return convertedDictionary;
        }
    }
}
