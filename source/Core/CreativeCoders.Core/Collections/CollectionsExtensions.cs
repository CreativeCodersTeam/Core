using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace CreativeCoders.Core.Collections;

[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
public static class CollectionsExtensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An IEnumerable extension method that do a fast count. The method first tries if
    ///     <paramref name="items"/> is a collection and reads Count if possible.
    /// </summary>
    ///
    /// <param name="items">    The items to act on. </param>
    /// <param name="maxCount"> (Optional) Number of maximum count if <paramref name="items"/> must
    ///                         be enumerated for  counting. </param>
    ///
    /// <returns>   The count. </returns>
    ///-------------------------------------------------------------------------------------------------
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static int FastCount(this IEnumerable items, int maxCount = 0)
    {
        return items.TryGetCollectionCount(out var count)
            ? count
            : items.EnumerableCount(maxCount);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An IEnumerable extension method that fast checks if the items count is in range.
    /// </summary>
    ///
    /// <param name="items">    The items to act on. </param>
    /// <param name="minCount"> Number of minimum count. </param>
    /// <param name="maxCount"> Number of maximum count. </param>
    ///
    /// <returns>   True if items count is in range, false otherwise. </returns>
    ///-------------------------------------------------------------------------------------------------
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

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An IEnumerable extension method that fast empty. </summary>
    ///
    /// <param name="items">    The items to act on. </param>
    ///
    /// <returns>   True if it succeeds, false if it fails. </returns>
    ///-------------------------------------------------------------------------------------------------
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

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An IEnumerable extension method that attempts to get the collection count from the
    ///     given IEnumerable.
    /// </summary>
    ///
    /// <param name="items">    The items to act on. </param>
    /// <param name="count">    [out] Count in items. </param>
    ///
    /// <returns>   True if it Count can be read from a collection, false otherwise. </returns>
    ///-------------------------------------------------------------------------------------------------
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
