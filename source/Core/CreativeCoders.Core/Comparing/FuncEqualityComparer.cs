using System;
using System.Collections.Generic;

namespace CreativeCoders.Core.Comparing;

/// <summary>
/// Implements <see cref="IEqualityComparer{T}"/> by extracting a key from each object using a delegate function
/// and comparing the keys for equality.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey">The type of the key used for equality comparison.</typeparam>
public class FuncEqualityComparer<T, TKey> : IEqualityComparer<T>
{
    private readonly Func<T, TKey> _keySelector;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncEqualityComparer{T, TKey}"/> class.
    /// </summary>
    /// <param name="keySelector">Function that extracts the equality key from an object.</param>
    public FuncEqualityComparer(Func<T, TKey> keySelector)
    {
        Ensure.IsNotNull(keySelector, nameof(keySelector));

        _keySelector = keySelector;
    }

    /// <inheritdoc/>
    public bool Equals(T x, T y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        var xValue = _keySelector(x);
        var yValue = _keySelector(y);

        return EqualityComparer<TKey>.Default.Equals(xValue, yValue);
    }

    /// <inheritdoc/>
    public int GetHashCode(T obj)
    {
        return _keySelector(obj)?.GetHashCode() ?? 0;
    }
}
