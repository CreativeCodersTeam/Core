using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core.Comparing;

/// <summary>
/// Implements <see cref="IComparer{T}"/> by extracting a key from each object using a delegate function
/// and comparing the keys with respect to a specified <see cref="SortOrder"/>.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey">The type of the key used for comparison.</typeparam>
public class FuncComparer<T, TKey> : IComparer<T>
{
    private readonly Func<T, TKey> _keySelector;

    private readonly SortOrder _sortOrder;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuncComparer{T, TKey}"/> class.
    /// </summary>
    /// <param name="keySelector">Function that extracts the comparison key from an object.</param>
    /// <param name="sortOrder">The direction in which to sort.</param>
    public FuncComparer(Func<T, TKey> keySelector, SortOrder sortOrder)
    {
        Ensure.IsNotNull(keySelector, nameof(keySelector));

        _keySelector = keySelector;
        _sortOrder = sortOrder;
    }

    /// <inheritdoc/>
    [SuppressMessage("ReSharper", "CompareNonConstrainedGenericWithNull")]
    public int Compare(T x, T y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (x == null)
        {
            return GetCompareResult(-1);
        }

        if (y == null)
        {
            return GetCompareResult(1);
        }

        var xValue = _keySelector(x);
        var yValue = _keySelector(y);

        return GetCompareResult(Comparer<TKey>.Default.Compare(xValue, yValue));
    }

    private int GetCompareResult(int compareResult)
    {
        return _sortOrder == SortOrder.Ascending ? compareResult : compareResult * -1;
    }
}
