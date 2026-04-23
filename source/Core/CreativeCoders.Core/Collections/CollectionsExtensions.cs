using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace CreativeCoders.Core.Collections;

/// <summary>
///     Provides fast counting and emptiness-check extension methods for <see cref="IEnumerable"/> sequences.
/// </summary>
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
public static class CollectionsExtensions
{
    /// <summary>
    ///     Counts the elements in the source sequence using the fastest available strategy.
    ///     If <paramref name="items"/> implements a known collection interface, the <c>Count</c>
    ///     property is read directly; otherwise the sequence is enumerated.
    /// </summary>
    /// <param name="items">The source sequence to count.</param>
    /// <param name="maxCount">
    ///     Maximum number of elements to count when the sequence must be enumerated.
    ///     A value of <c>0</c> means no limit.
    /// </param>
    /// <returns>The number of elements in the sequence, capped at <paramref name="maxCount"/> when applicable.</returns>
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static int FastCount(this IEnumerable items, int maxCount = 0)
    {
        return items.TryGetCollectionCount(out var count)
            ? count
            : items.EnumerableCount(maxCount);
    }

    /// <summary>
    ///     Determines whether the number of elements in the source sequence falls within the specified range,
    ///     using the fastest available counting strategy.
    /// </summary>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="minCount">The inclusive lower bound of the acceptable count.</param>
    /// <param name="maxCount">The exclusive upper bound of the acceptable count.</param>
    /// <returns>
    ///     <see langword="true"/> if the element count is greater than or equal to <paramref name="minCount"/>
    ///     and less than <paramref name="maxCount"/>; otherwise, <see langword="false"/>.
    /// </returns>
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static bool FastCountInRange(this IEnumerable items, int minCount, int maxCount)
    {
        return items.TryGetCollectionCount(out var count)
            ? count >= minCount && count <= maxCount
            : items.EnumerableCountInRange(minCount, maxCount);
    }

    private static bool EnumerableCountInRange(this IEnumerable items, int minCount, int maxCount)
    {
        var count = 0;

        foreach (var _ in items)
        {
            count++;

            if (count >= maxCount)
            {
                return false;
            }
        }

        return count >= minCount;
    }

    private static int EnumerableCount(this IEnumerable items, int maxCount)
    {
        var count = 0;

        foreach (var _ in items)
        {
            count++;

            if (maxCount > 0 && count >= maxCount)
            {
                break;
            }
        }

        return count;
    }

    /// <summary>
    ///     Determines whether the source sequence contains no elements, using the fastest available strategy.
    /// </summary>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <returns>
    ///     <see langword="true"/> if the sequence is empty; otherwise, <see langword="false"/>.
    /// </returns>
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static bool FastEmpty(this IEnumerable items)
    {
        return items.TryGetCollectionCount(out var count)
            ? count == 0
            : !MoveNext();

        bool MoveNext()
        {
            var enumerator = items.GetEnumerator();

            var result = enumerator.MoveNext();

            (enumerator as IDisposable)?.Dispose();

            return result;
        }
    }

    /// <summary>
    ///     Attempts to retrieve the element count directly from a known collection interface
    ///     without enumerating the sequence.
    /// </summary>
    /// <param name="items">The source sequence to inspect.</param>
    /// <param name="count">
    ///     When this method returns <see langword="true"/>, contains the number of elements;
    ///     otherwise, <c>0</c>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the count was obtained from a collection property;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetCollectionCount(this IEnumerable items, out int count)
    {
        switch (items)
        {
            case IReadOnlyCollection<object> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<byte> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<short> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<ushort> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<int> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<uint> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<long> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<ulong> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<double> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<float> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<bool> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case IReadOnlyCollection<DateTime> readOnlyCollection:
                count = readOnlyCollection.Count;
                break;
            case string text:
                count = text.Length;
                break;
            default:
                count = 0;
                return false;
        }

        return true;
    }
}
