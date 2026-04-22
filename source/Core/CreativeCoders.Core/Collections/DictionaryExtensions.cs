using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Collections;

/// <summary>
///     Provides extension methods for <see cref="IDictionary{TKey,TValue}"/> and <see cref="IDictionary"/>,
///     including reverse-lookup and typed conversion operations.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    ///     Returns the key associated with the specified value in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to search.</param>
    /// <param name="value">The value whose associated key is returned.</param>
    /// <returns>The key associated with <paramref name="value"/>.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     No entry with the specified <paramref name="value"/> exists in the dictionary.
    /// </exception>
    public static TKey GetKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
    {
        if (TryGetKeyByValue(dictionary, value, out var key))
        {
            return key;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    ///     Attempts to find the key associated with the specified value in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to search.</param>
    /// <param name="value">The value whose associated key is returned.</param>
    /// <param name="key">
    ///     When this method returns <see langword="true"/>, contains the key associated with <paramref name="value"/>;
    ///     otherwise, the default value of <typeparamref name="TKey"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a matching entry was found; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetKeyByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value,
        out TKey key)
    {
        foreach (var dictEntry in dictionary.Where(dictEntry =>
                     (dictEntry.Value is null && value is null) || dictEntry.Value?.Equals(value) == true))
        {
            key = dictEntry.Key;
            return true;
        }

        key = default;
        return false;
    }

    /// <summary>
    ///     Converts a non-generic <see cref="IDictionary"/> to a strongly-typed
    ///     <see cref="IDictionary{TKey,TValue}"/> by casting each key and value.
    /// </summary>
    /// <typeparam name="TKey">The target key type.</typeparam>
    /// <typeparam name="TValue">The target value type.</typeparam>
    /// <param name="dictionary">The non-generic dictionary to convert.</param>
    /// <param name="skipNotMatchingEntries">
    ///     <see langword="true"/> to silently skip entries whose key or value cannot be cast;
    ///     otherwise, <see langword="false"/> to throw an <see cref="InvalidCastException"/>.
    /// </param>
    /// <returns>A new strongly-typed dictionary containing the converted entries.</returns>
    /// <exception cref="InvalidCastException">
    ///     <paramref name="skipNotMatchingEntries"/> is <see langword="false"/> and an entry's key or value
    ///     cannot be cast to the target type.
    /// </exception>
    public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IDictionary dictionary,
        bool skipNotMatchingEntries)
    {
        var convertedDictionary = new Dictionary<TKey, TValue>();

        foreach (DictionaryEntry dictionaryEntry in dictionary)
        {
            if (dictionaryEntry.Key is not TKey key)
            {
                if (skipNotMatchingEntries)
                {
                    continue;
                }

                throw new InvalidCastException("Base dictionary key has wrong type");
            }

            if (dictionaryEntry.Value is not TValue value)
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

    /// <summary>
    ///     Converts a non-generic <see cref="IDictionary"/> to a strongly-typed
    ///     <see cref="IDictionary{TKey,TValue}"/> by casting each key and transforming each value
    ///     with the specified selector.
    /// </summary>
    /// <typeparam name="TKey">The target key type.</typeparam>
    /// <typeparam name="TValue">The target value type.</typeparam>
    /// <param name="dictionary">The non-generic dictionary to convert.</param>
    /// <param name="valueSelector">The function that transforms each raw value into <typeparamref name="TValue"/>.</param>
    /// <param name="skipNotMatchingEntries">
    ///     <see langword="true"/> to silently skip entries whose key cannot be cast;
    ///     otherwise, <see langword="false"/> to throw an <see cref="InvalidCastException"/>.
    /// </param>
    /// <returns>A new strongly-typed dictionary containing the converted entries.</returns>
    /// <exception cref="InvalidCastException">
    ///     <paramref name="skipNotMatchingEntries"/> is <see langword="false"/> and an entry's key
    ///     cannot be cast to <typeparamref name="TKey"/>.
    /// </exception>
    public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IDictionary dictionary,
        Func<object, TValue> valueSelector, bool skipNotMatchingEntries)
    {
        var convertedDictionary = new Dictionary<TKey, TValue>();

        foreach (DictionaryEntry dictionaryEntry in dictionary)
        {
            if (dictionaryEntry.Key is not TKey key)
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
