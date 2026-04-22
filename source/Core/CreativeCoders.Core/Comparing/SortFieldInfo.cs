using System;

namespace CreativeCoders.Core.Comparing;

/// <summary>
/// Represents a sort field definition consisting of a key selector function and a sort order.
/// </summary>
/// <typeparam name="T">The type of the object to extract the sort key from.</typeparam>
/// <typeparam name="TKey">The type of the sort key.</typeparam>
public class SortFieldInfo<T, TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SortFieldInfo{T, TKey}"/> class.
    /// </summary>
    /// <param name="keySelector">Function that extracts the sort key from an object.</param>
    /// <param name="sortOrder">The direction in which to sort.</param>
    public SortFieldInfo(Func<T, TKey> keySelector, SortOrder sortOrder)
    {
        KeySelector = keySelector;
        SortOrder = sortOrder;
    }

    /// <summary>
    /// Gets the function that extracts the sort key from an object.
    /// </summary>
    public Func<T, TKey> KeySelector { get; }

    /// <summary>
    /// Gets the direction in which to sort.
    /// </summary>
    public SortOrder SortOrder { get; }
}
