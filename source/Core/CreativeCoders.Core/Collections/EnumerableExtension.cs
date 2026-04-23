using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.Core.Comparing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Collections;

/// <summary>
///     Provides extension methods for <see cref="IEnumerable{T}"/> and <see cref="IEnumerable"/>
///     sequences, including iteration, filtering, projection, sorting, and duplicate detection.
/// </summary>
[PublicAPI]
public static class EnumerableExtension
{
    /// <summary>
    ///     Returns the single element of the source sequence, or the default value if the sequence
    ///     contains zero elements or more than one element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <returns>
    ///     The single element when the sequence contains exactly one element;
    ///     otherwise, the default value of <typeparamref name="T"/>.
    /// </returns>
    public static T OnlySingleOrDefault<T>(this IEnumerable<T> items)
    {
        using var enumerator = items.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return default;
        }

        var firstElement = enumerator.Current;

        return enumerator.MoveNext()
            ? default
            : firstElement;
    }

    /// <summary>
    ///     Executes the specified action on each element of the source sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="self">The source sequence to iterate.</param>
    /// <param name="action">The action to execute on each element.</param>
    public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
        Ensure.IsNotNull(action);

        foreach (var item in self)
        {
            action(item);
        }
    }

    /// <summary>
    ///     Executes the specified action on each element of the non-generic source sequence
    ///     that can be cast to <typeparamref name="T"/>. Elements that cannot be cast are skipped.
    /// </summary>
    /// <typeparam name="T">The target type to which elements are cast before invoking the action.</typeparam>
    /// <param name="self">The non-generic source sequence to iterate.</param>
    /// <param name="action">The action to execute on each matching element.</param>
    public static void ForEach<T>(this IEnumerable self, Action<T> action)
    {
        Ensure.IsNotNull(action);

        foreach (var obj in self)
        {
            if (obj is T variable)
            {
                action(variable);
            }
        }
    }

    /// <summary>
    ///     Executes the specified action on each element of the source sequence,
    ///     providing the zero-based index of each element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence to iterate.</param>
    /// <param name="action">The action to execute, receiving the element and its zero-based index.</param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        Ensure.IsNotNull(action);

        var index = 0;
        foreach (var element in source)
        {
            action(element, index++);
        }
    }

    /// <summary>
    ///     Executes the specified action on each element of the non-generic source sequence,
    ///     providing the zero-based index of each element.
    /// </summary>
    /// <param name="source">The non-generic source sequence to iterate.</param>
    /// <param name="action">The action to execute, receiving the element and its zero-based index.</param>
    public static void ForEach(this IEnumerable source, Action<object, int> action)
    {
        Ensure.IsNotNull(action);

        var index = 0;
        foreach (var element in source)
        {
            action(element, index++);
        }
    }

    /// <summary>
    ///     Executes the specified action on each element of the array that can be cast to
    ///     <typeparamref name="T"/>. Elements that cannot be cast are skipped.
    /// </summary>
    /// <typeparam name="T">The target type to which elements are cast before invoking the action.</typeparam>
    /// <param name="self">The source array to iterate.</param>
    /// <param name="action">The action to execute on each matching element.</param>
    public static void ForEach<T>(this Array self, Action<T> action)
    {
        Ensure.IsNotNull(action);

        (self as IEnumerable).ForEach(action);
    }

    /// <summary>
    ///     Asynchronously executes the specified function on each element of the source sequence, awaiting
    ///     each invocation sequentially.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="self">The source sequence to iterate.</param>
    /// <param name="actionAsync">The asynchronous function to execute on each element.</param>
    /// <returns>A <see cref="Task"/> that completes when all elements have been processed.</returns>
    public static async Task ForEachAsync<T>(this IEnumerable<T> self, Func<T, Task> actionAsync)
    {
        Ensure.IsNotNull(actionAsync);

        foreach (var item in self)
        {
            await actionAsync(item).ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Asynchronously executes the specified function on each element of the source sequence,
    ///     providing the zero-based index and awaiting each invocation sequentially.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="source">The source sequence to iterate.</param>
    /// <param name="actionAsync">The asynchronous function to execute, receiving the element and its zero-based index.</param>
    /// <returns>A <see cref="Task"/> that completes when all elements have been processed.</returns>
    public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, int, Task> actionAsync)
    {
        Ensure.IsNotNull(actionAsync);

        var index = 0;
        foreach (var element in source)
        {
            await actionAsync(element, index++).ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Executes a side-effect action on each element as it passes through the sequence,
    ///     yielding every element unchanged. The action is invoked lazily during enumeration.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to observe.</param>
    /// <param name="pipeAction">The action to execute on each element for side effects.</param>
    /// <returns>A sequence containing all elements of <paramref name="items"/> in their original order.</returns>
    public static IEnumerable<T> Pipe<T>(this IEnumerable<T> items, Action<T> pipeAction)
    {
        Ensure.IsNotNull(pipeAction);

        return PipeCore();

        IEnumerable<T> PipeCore()
        {
            foreach (var item in items)
            {
                pipeAction(item);
                yield return item;
            }
        }
    }

    /// <summary>
    ///     Returns elements from the source sequence until the predicate is satisfied.
    ///     The element that satisfies the predicate is included in the result.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to take from.</param>
    /// <param name="predicate">
    ///     The function that tests each element.
    ///     Enumeration stops after the first element for which this returns <see langword="true"/>.
    /// </param>
    /// <returns>
    ///     A sequence containing elements up to and including the first element that satisfies
    ///     <paramref name="predicate"/>.
    /// </returns>
    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        Ensure.IsNotNull(predicate);

        return TakeUntilCore();

        IEnumerable<T> TakeUntilCore()
        {
            foreach (var item in items)
            {
                yield return item;

                if (predicate(item))
                {
                    yield break;
                }
            }
        }
    }

    /// <summary>
    ///     Skips elements from the source sequence until the predicate is satisfied,
    ///     then yields all remaining elements. The element that satisfies the predicate is not included.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to skip from.</param>
    /// <param name="predicate">
    ///     The function that tests each element.
    ///     Elements are skipped until this returns <see langword="true"/>; subsequent elements are yielded.
    /// </param>
    /// <returns>A sequence of elements following the first element that satisfies <paramref name="predicate"/>.</returns>
    public static IEnumerable<T> SkipUntil<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        Ensure.IsNotNull(predicate);

        return SkipUntilCore();

        IEnumerable<T> SkipUntilCore()
        {
            var checkUntil = true;

            foreach (var item in items)
            {
                if (checkUntil)
                {
                    if (predicate(item))
                    {
                        checkUntil = false;
                    }

                    continue;
                }

                yield return item;
            }
        }
    }

    /// <summary>
    ///     Returns every <paramref name="step"/>-th element from the source sequence,
    ///     starting with the first element (index 0).
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence.</param>
    /// <param name="step">The interval between selected elements.</param>
    /// <returns>A sequence containing every <paramref name="step"/>-th element.</returns>
    public static IEnumerable<T> TakeEvery<T>(this IEnumerable<T> items, int step)
    {
        return items.Where((_, index) => index % step == 0);
    }

    /// <summary>
    ///     Removes all elements in <paramref name="removeEntries"/> from the source collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="self">The collection from which elements are removed.</param>
    /// <param name="removeEntries">The elements to remove.</param>
    public static void Remove<T>(this ICollection<T> self, IEnumerable<T> removeEntries)
    {
        Ensure.IsNotNull(removeEntries);

        foreach (var removeEntry in removeEntries)
        {
            self.Remove(removeEntry);
        }
    }

    /// <summary>
    ///     Removes all elements that match the specified predicate from the source collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="self">The collection from which elements are removed.</param>
    /// <param name="predicate">The condition that determines which elements to remove.</param>
    public static void Remove<T>(this ICollection<T> self, Predicate<T> predicate)
    {
        Ensure.IsNotNull(predicate);

        var removeEntries = self.Where(item => predicate(item)).ToArray();
        self.Remove(removeEntries);
    }

    /// <summary>
    ///     Filters out <see langword="null"/> elements from the source sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="self">The source sequence to filter.</param>
    /// <returns>A sequence containing only the non-<see langword="null"/> elements.</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> self)
        where T : class
    {
        return self.Where(item => item != null);
    }

    /// <summary>
    ///     Determines whether the source sequence contains exactly one element that satisfies the predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="predicate">The condition to test each element against.</param>
    /// <returns>
    ///     <see langword="true"/> if exactly one element satisfies <paramref name="predicate"/>;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsSingle<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        Ensure.IsNotNull(predicate);

        var num = 0;
        // ReSharper disable once LoopCanBeConvertedToQuery

        foreach (var _ in items.Where(predicate))
        {
            // ReSharper disable once InvertIf
            if (++num > 1)
            {
                return false;
            }
        }

        return num == 1;
    }

    /// <summary>
    ///     Determines whether the source sequence contains exactly one element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <returns>
    ///     <see langword="true"/> if the sequence contains exactly one element;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsSingle<T>(this IEnumerable<T> items)
    {
        return items.IsSingle(_ => true);
    }

    /// <summary>
    ///     Returns distinct elements from the source sequence by comparing a single key.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">The type of the key used for equality comparison.</typeparam>
    /// <param name="items">The source sequence.</param>
    /// <param name="keySelector">The function that extracts the comparison key from each element.</param>
    /// <returns>A sequence of distinct elements based on the selected key.</returns>
    public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
    {
        return items.Distinct(new FuncEqualityComparer<T, TKey>(keySelector));
    }

    /// <summary>
    ///     Returns distinct elements from the source sequence by comparing two keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first comparison key.</typeparam>
    /// <typeparam name="TKey2">The type of the second comparison key.</typeparam>
    /// <param name="items">The source sequence.</param>
    /// <param name="keySelector1">The function that extracts the first comparison key.</param>
    /// <param name="keySelector2">The function that extracts the second comparison key.</param>
    /// <returns>A sequence of distinct elements based on the selected keys.</returns>
    public static IEnumerable<T> Distinct<T, TKey1, TKey2>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2)
    {
        return items.Distinct(new MultiFuncEqualityComparer<T, TKey1, TKey2>(keySelector1, keySelector2));
    }

    /// <summary>
    ///     Returns distinct elements from the source sequence by comparing three keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first comparison key.</typeparam>
    /// <typeparam name="TKey2">The type of the second comparison key.</typeparam>
    /// <typeparam name="TKey3">The type of the third comparison key.</typeparam>
    /// <param name="items">The source sequence.</param>
    /// <param name="keySelector1">The function that extracts the first comparison key.</param>
    /// <param name="keySelector2">The function that extracts the second comparison key.</param>
    /// <param name="keySelector3">The function that extracts the third comparison key.</param>
    /// <returns>A sequence of distinct elements based on the selected keys.</returns>
    public static IEnumerable<T> Distinct<T, TKey1, TKey2, TKey3>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2, Func<T, TKey3> keySelector3)
    {
        return items.Distinct(
            new MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3>(keySelector1, keySelector2,
                keySelector3));
    }

    /// <summary>
    ///     Returns distinct elements from the source sequence by comparing four keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first comparison key.</typeparam>
    /// <typeparam name="TKey2">The type of the second comparison key.</typeparam>
    /// <typeparam name="TKey3">The type of the third comparison key.</typeparam>
    /// <typeparam name="TKey4">The type of the fourth comparison key.</typeparam>
    /// <param name="items">The source sequence.</param>
    /// <param name="keySelector1">The function that extracts the first comparison key.</param>
    /// <param name="keySelector2">The function that extracts the second comparison key.</param>
    /// <param name="keySelector3">The function that extracts the third comparison key.</param>
    /// <param name="keySelector4">The function that extracts the fourth comparison key.</param>
    /// <returns>A sequence of distinct elements based on the selected keys.</returns>
    public static IEnumerable<T> Distinct<T, TKey1, TKey2, TKey3, TKey4>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2, Func<T, TKey3> keySelector3,
        Func<T, TKey4> keySelector4)
    {
        return items.Distinct(
            new MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4>(keySelector1, keySelector2,
                keySelector3, keySelector4));
    }

    /// <summary>
    ///     Returns distinct elements from the source sequence by comparing five keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first comparison key.</typeparam>
    /// <typeparam name="TKey2">The type of the second comparison key.</typeparam>
    /// <typeparam name="TKey3">The type of the third comparison key.</typeparam>
    /// <typeparam name="TKey4">The type of the fourth comparison key.</typeparam>
    /// <typeparam name="TKey5">The type of the fifth comparison key.</typeparam>
    /// <param name="items">The source sequence.</param>
    /// <param name="keySelector1">The function that extracts the first comparison key.</param>
    /// <param name="keySelector2">The function that extracts the second comparison key.</param>
    /// <param name="keySelector3">The function that extracts the third comparison key.</param>
    /// <param name="keySelector4">The function that extracts the fourth comparison key.</param>
    /// <param name="keySelector5">The function that extracts the fifth comparison key.</param>
    /// <returns>A sequence of distinct elements based on the selected keys.</returns>
    public static IEnumerable<T> Distinct<T, TKey1, TKey2, TKey3, TKey4, TKey5>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2, Func<T, TKey3> keySelector3,
        Func<T, TKey4> keySelector4, Func<T, TKey5> keySelector5)
    {
        return items.Distinct(
            new MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4, TKey5>(keySelector1, keySelector2,
                keySelector3, keySelector4, keySelector5));
    }

    /// <summary>
    ///     Returns distinct elements from the source sequence by comparing an arbitrary number
    ///     of keys of the same type.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">The type of each comparison key.</typeparam>
    /// <param name="items">The source sequence.</param>
    /// <param name="keySelectors">The functions that extract comparison keys from each element.</param>
    /// <returns>A sequence of distinct elements based on the selected keys.</returns>
    public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> items,
        params Func<T, TKey>[] keySelectors)
    {
        return items.Distinct(new MultiFuncEqualityComparer<T, TKey>(keySelectors));
    }

    /// <summary>
    ///     Returns all duplicate elements from the source sequence, compared using the default
    ///     equality comparer for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <returns>A sequence containing all elements that appear more than once.</returns>
    public static IEnumerable<T> NotDistinct<T>(this IEnumerable<T> items)
    {
        return items.NotDistinct(x => x);
    }

    /// <summary>
    ///     Returns all duplicate elements from the source sequence, compared by a single key.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">The type of the key used for equality comparison.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="keySelector">The function that extracts the comparison key from each element.</param>
    /// <returns>A sequence containing all elements whose key appears more than once.</returns>
    public static IEnumerable<T> NotDistinct<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
    {
        return items.NotDistinct(new FuncEqualityComparer<T, TKey>(keySelector));
    }

    /// <summary>
    ///     Returns all duplicate elements from the source sequence, compared by two keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first comparison key.</typeparam>
    /// <typeparam name="TKey2">The type of the second comparison key.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="keySelector1">The function that extracts the first comparison key.</param>
    /// <param name="keySelector2">The function that extracts the second comparison key.</param>
    /// <returns>A sequence containing all elements whose combined keys appear more than once.</returns>
    public static IEnumerable<T> NotDistinct<T, TKey1, TKey2>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2)
    {
        return items.NotDistinct(
            new MultiFuncEqualityComparer<T, TKey1, TKey2>(keySelector1, keySelector2));
    }

    /// <summary>
    ///     Returns all duplicate elements from the source sequence, compared by three keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first comparison key.</typeparam>
    /// <typeparam name="TKey2">The type of the second comparison key.</typeparam>
    /// <typeparam name="TKey3">The type of the third comparison key.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="keySelector1">The function that extracts the first comparison key.</param>
    /// <param name="keySelector2">The function that extracts the second comparison key.</param>
    /// <param name="keySelector3">The function that extracts the third comparison key.</param>
    /// <returns>A sequence containing all elements whose combined keys appear more than once.</returns>
    public static IEnumerable<T> NotDistinct<T, TKey1, TKey2, TKey3>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2, Func<T, TKey3> keySelector3)
    {
        return items.NotDistinct(
            new MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3>(keySelector1, keySelector2,
                keySelector3));
    }

    /// <summary>
    ///     Returns all duplicate elements from the source sequence, compared by four keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first comparison key.</typeparam>
    /// <typeparam name="TKey2">The type of the second comparison key.</typeparam>
    /// <typeparam name="TKey3">The type of the third comparison key.</typeparam>
    /// <typeparam name="TKey4">The type of the fourth comparison key.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="keySelector1">The function that extracts the first comparison key.</param>
    /// <param name="keySelector2">The function that extracts the second comparison key.</param>
    /// <param name="keySelector3">The function that extracts the third comparison key.</param>
    /// <param name="keySelector4">The function that extracts the fourth comparison key.</param>
    /// <returns>A sequence containing all elements whose combined keys appear more than once.</returns>
    public static IEnumerable<T> NotDistinct<T, TKey1, TKey2, TKey3, TKey4>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2, Func<T, TKey3> keySelector3,
        Func<T, TKey4> keySelector4)
    {
        return items.NotDistinct(
            new MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4>(keySelector1, keySelector2,
                keySelector3, keySelector4));
    }

    /// <summary>
    ///     Returns all duplicate elements from the source sequence, compared by five keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first comparison key.</typeparam>
    /// <typeparam name="TKey2">The type of the second comparison key.</typeparam>
    /// <typeparam name="TKey3">The type of the third comparison key.</typeparam>
    /// <typeparam name="TKey4">The type of the fourth comparison key.</typeparam>
    /// <typeparam name="TKey5">The type of the fifth comparison key.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="keySelector1">The function that extracts the first comparison key.</param>
    /// <param name="keySelector2">The function that extracts the second comparison key.</param>
    /// <param name="keySelector3">The function that extracts the third comparison key.</param>
    /// <param name="keySelector4">The function that extracts the fourth comparison key.</param>
    /// <param name="keySelector5">The function that extracts the fifth comparison key.</param>
    /// <returns>A sequence containing all elements whose combined keys appear more than once.</returns>
    public static IEnumerable<T> NotDistinct<T, TKey1, TKey2, TKey3, TKey4, TKey5>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2, Func<T, TKey3> keySelector3,
        Func<T, TKey4> keySelector4, Func<T, TKey5> keySelector5)
    {
        return items.NotDistinct(
            new MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4, TKey5>(keySelector1, keySelector2,
                keySelector3, keySelector4, keySelector5));
    }

    /// <summary>
    ///     Returns all duplicate elements from the source sequence, compared by an arbitrary number
    ///     of keys of the same type.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey">The type of each comparison key.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="keySelectors">The functions that extract comparison keys from each element.</param>
    /// <returns>A sequence containing all elements whose combined keys appear more than once.</returns>
    public static IEnumerable<T> NotDistinct<T, TKey>(this IEnumerable<T> items,
        params Func<T, TKey>[] keySelectors)
    {
        return items.NotDistinct(new MultiFuncEqualityComparer<T, TKey>(keySelectors));
    }

    /// <summary>
    ///     Returns all duplicate elements from the source sequence, using the specified equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to evaluate.</param>
    /// <param name="comparer">The equality comparer used to detect duplicates.</param>
    /// <returns>A sequence containing all elements that appear more than once according to <paramref name="comparer"/>.</returns>
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static IEnumerable<T> NotDistinct<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer)
    {
        Ensure.NotNull(items);
        Ensure.NotNull(comparer);

        return NotDistinctCore();

        IEnumerable<T> NotDistinctCore()
        {
            var bufferedItems = items.SelectWithIndex().ToArray();
            var foundDuplicates = new List<int>();

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var item in bufferedItems)
            {
                var duplicates = bufferedItems
                    .Where(x =>
                        !foundDuplicates.Contains(x.Index) && comparer.Equals(item.Data, x.Data))
                    .ToArray();

                // ReSharper disable once InvertIf
                if (duplicates.Length > 1)
                {
                    foundDuplicates.AddRange(duplicates.Select(x => x.Index));

                    foreach (var duplicate in duplicates)
                    {
                        yield return duplicate.Data;
                    }
                }
            }
        }
    }

    /// <summary>
    ///     Projects each element of the source sequence into an <see cref="ItemWithIndex{T}"/>
    ///     that pairs the element with its zero-based index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="items">The source sequence to project.</param>
    /// <returns>A sequence of <see cref="ItemWithIndex{T}"/> instances.</returns>
    public static IEnumerable<ItemWithIndex<T>> SelectWithIndex<T>(this IEnumerable<T> items)
    {
        return items.Select((x, index) => new ItemWithIndex<T>(index, x));
    }

    /// <summary>
    ///     Sorts the source sequence in ascending order by two keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first sort key.</typeparam>
    /// <typeparam name="TKey2">The type of the second sort key.</typeparam>
    /// <param name="items">The source sequence to sort.</param>
    /// <param name="keySelector1">The function that extracts the first sort key.</param>
    /// <param name="keySelector2">The function that extracts the second sort key.</param>
    /// <returns>An ordered sequence sorted in ascending order by both keys.</returns>
    public static IOrderedEnumerable<T> OrderBy<T, TKey1, TKey2>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1,
        Func<T, TKey2> keySelector2)
    {
        return items.Sort(new SortFieldInfo<T, TKey1>(keySelector1, SortOrder.Ascending),
            new SortFieldInfo<T, TKey2>(keySelector2, SortOrder.Ascending));
    }

    /// <summary>
    ///     Sorts the source sequence in descending order by two keys.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first sort key.</typeparam>
    /// <typeparam name="TKey2">The type of the second sort key.</typeparam>
    /// <param name="items">The source sequence to sort.</param>
    /// <param name="keySelector1">The function that extracts the first sort key.</param>
    /// <param name="keySelector2">The function that extracts the second sort key.</param>
    /// <returns>An ordered sequence sorted in descending order by both keys.</returns>
    public static IOrderedEnumerable<T> OrderByDescending<T, TKey1, TKey2>(this IEnumerable<T> items,
        Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2)
    {
        return items.Sort(new SortFieldInfo<T, TKey1>(keySelector1, SortOrder.Descending),
            new SortFieldInfo<T, TKey2>(keySelector2, SortOrder.Descending));
    }

    /// <summary>
    ///     Sorts the source sequence using two sort field descriptors that each specify a key and sort order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first sort key.</typeparam>
    /// <typeparam name="TKey2">The type of the second sort key.</typeparam>
    /// <param name="items">The source sequence to sort.</param>
    /// <param name="sortFieldInfo1">The first sort field descriptor.</param>
    /// <param name="sortFieldInfo2">The second sort field descriptor.</param>
    /// <returns>An ordered sequence sorted according to the specified sort field descriptors.</returns>
    public static IOrderedEnumerable<T> Sort<T, TKey1, TKey2>(this IEnumerable<T> items,
        SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2)
    {
        return items.OrderBy(x => x, new MultiFuncComparer<T, TKey1, TKey2>(sortFieldInfo1, sortFieldInfo2));
    }

    /// <summary>
    ///     Sorts the source sequence using three sort field descriptors that each specify a key and sort order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first sort key.</typeparam>
    /// <typeparam name="TKey2">The type of the second sort key.</typeparam>
    /// <typeparam name="TKey3">The type of the third sort key.</typeparam>
    /// <param name="items">The source sequence to sort.</param>
    /// <param name="sortFieldInfo1">The first sort field descriptor.</param>
    /// <param name="sortFieldInfo2">The second sort field descriptor.</param>
    /// <param name="sortFieldInfo3">The third sort field descriptor.</param>
    /// <returns>An ordered sequence sorted according to the specified sort field descriptors.</returns>
    public static IOrderedEnumerable<T> Sort<T, TKey1, TKey2, TKey3>(this IEnumerable<T> items,
        SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
        SortFieldInfo<T, TKey3> sortFieldInfo3)
    {
        return items.OrderBy(x => x,
            new MultiFuncComparer<T, TKey1, TKey2, TKey3>(sortFieldInfo1, sortFieldInfo2, sortFieldInfo3));
    }

    /// <summary>
    ///     Sorts the source sequence using four sort field descriptors that each specify a key and sort order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first sort key.</typeparam>
    /// <typeparam name="TKey2">The type of the second sort key.</typeparam>
    /// <typeparam name="TKey3">The type of the third sort key.</typeparam>
    /// <typeparam name="TKey4">The type of the fourth sort key.</typeparam>
    /// <param name="items">The source sequence to sort.</param>
    /// <param name="sortFieldInfo1">The first sort field descriptor.</param>
    /// <param name="sortFieldInfo2">The second sort field descriptor.</param>
    /// <param name="sortFieldInfo3">The third sort field descriptor.</param>
    /// <param name="sortFieldInfo4">The fourth sort field descriptor.</param>
    /// <returns>An ordered sequence sorted according to the specified sort field descriptors.</returns>
    public static IOrderedEnumerable<T> Sort<T, TKey1, TKey2, TKey3, TKey4>(this IEnumerable<T> items,
        SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
        SortFieldInfo<T, TKey3> sortFieldInfo3, SortFieldInfo<T, TKey4> sortFieldInfo4)
    {
        return items.OrderBy(x => x,
            new MultiFuncComparer<T, TKey1, TKey2, TKey3, TKey4>(sortFieldInfo1, sortFieldInfo2,
                sortFieldInfo3,
                sortFieldInfo4));
    }

    /// <summary>
    ///     Sorts the source sequence using five sort field descriptors that each specify a key and sort order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <typeparam name="TKey1">The type of the first sort key.</typeparam>
    /// <typeparam name="TKey2">The type of the second sort key.</typeparam>
    /// <typeparam name="TKey3">The type of the third sort key.</typeparam>
    /// <typeparam name="TKey4">The type of the fourth sort key.</typeparam>
    /// <typeparam name="TKey5">The type of the fifth sort key.</typeparam>
    /// <param name="items">The source sequence to sort.</param>
    /// <param name="sortFieldInfo1">The first sort field descriptor.</param>
    /// <param name="sortFieldInfo2">The second sort field descriptor.</param>
    /// <param name="sortFieldInfo3">The third sort field descriptor.</param>
    /// <param name="sortFieldInfo4">The fourth sort field descriptor.</param>
    /// <param name="sortFieldInfo5">The fifth sort field descriptor.</param>
    /// <returns>An ordered sequence sorted according to the specified sort field descriptors.</returns>
    public static IOrderedEnumerable<T> Sort<T, TKey1, TKey2, TKey3, TKey4, TKey5>(this IEnumerable<T> items,
        SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
        SortFieldInfo<T, TKey3> sortFieldInfo3, SortFieldInfo<T, TKey4> sortFieldInfo4,
        SortFieldInfo<T, TKey5> sortFieldInfo5)
    {
        return items.OrderBy(x => x,
            new MultiFuncComparer<T, TKey1, TKey2, TKey3, TKey4, TKey5>(sortFieldInfo1, sortFieldInfo2,
                sortFieldInfo3, sortFieldInfo4, sortFieldInfo5));
    }

    /// <summary>
    ///     Filters and transforms elements from the source sequence in a single pass.
    ///     Each element is evaluated by <paramref name="choose"/>, and only elements for which
    ///     the function indicates selection are projected into the result.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of elements in the resulting sequence.</typeparam>
    /// <param name="items">The source sequence to filter and transform.</param>
    /// <param name="choose">
    ///     The function that evaluates each element, returning a tuple whose <c>IsChoosen</c> flag indicates
    ///     selection and whose <c>Value</c> contains the projected result.
    /// </param>
    /// <returns>A sequence of projected values for the chosen elements.</returns>
    public static IEnumerable<TResult> Choose<T, TResult>(this IEnumerable<T> items,
        Func<T, (bool IsChoosen, TResult Value)> choose)
    {
        Ensure.IsNotNull(choose);

        return items
            .Select(choose)
            .Where(x => x.IsChoosen)
            .Select(x => x.Value);
    }

#nullable enable
    /// <summary>
    ///     Filters the elements of the non-generic source sequence by the specified runtime
    ///     <see cref="Type"/>, using reflection to invoke <see cref="Enumerable.OfType{TResult}"/>.
    /// </summary>
    /// <param name="source">The non-generic source sequence to filter.</param>
    /// <param name="itemType">The runtime type to filter elements by.</param>
    /// <returns>
    ///     An <see cref="IEnumerable{T}"/> containing only the elements of <paramref name="itemType"/>,
    ///     or <see langword="null"/> if the method cannot be resolved.
    /// </returns>
    /// <exception cref="MissingMethodException">
    ///     The <see cref="Enumerable.OfType{TResult}"/> method cannot be found via reflection.
    /// </exception>
    public static object? OfType(this IEnumerable source, Type itemType)
    {
        var ofTypeMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.OfType))
            ?.MakeGenericMethod(itemType);

        return ofTypeMethod != null
            ? ofTypeMethod.Invoke(null, [source])
            : throw new MissingMethodException(nameof(Enumerable), nameof(Enumerable.OfType));
    }
    // ReSharper disable once UnusedNullableDirective
#nullable restore
}
