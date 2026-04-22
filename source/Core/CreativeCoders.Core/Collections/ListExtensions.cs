using System.Collections.Generic;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Collections;

/// <summary>
///     Provides extension methods for <see cref="IList{T}"/>, including bulk-add and replacement operations.
/// </summary>
[PublicAPI]
public static class ListExtensions
{
    /// <summary>
    ///     Adds all elements from the specified sequence to the end of the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The target list to add elements to.</param>
    /// <param name="items">The elements to add.</param>
    /// <returns>The same <paramref name="list"/> instance, for chaining.</returns>
    public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
        Ensure.IsNotNull(items, nameof(items));

        items.ForEach(list.Add);

        return list;
    }

    /// <summary>
    ///     Adds the specified elements to the end of the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The target list to add elements to.</param>
    /// <param name="items">The elements to add.</param>
    /// <returns>The same <paramref name="list"/> instance, for chaining.</returns>
    public static IList<T> AddRange<T>(this IList<T> list, params T[] items)
    {
        Ensure.IsNotNull(items, nameof(items));

        items.ForEach(list.Add);

        return list;
    }

    /// <summary>
    ///     Replaces all elements in the list with the elements from the specified sequence.
    ///     The list is cleared before the new elements are added.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The target list whose contents are replaced.</param>
    /// <param name="items">The new elements to populate the list with.</param>
    public static void SetItems<T>(this IList<T> list, IEnumerable<T> items)
    {
        Ensure.IsNotNull(items, nameof(items));

        list.Clear();
        list.AddRange(items);
    }
}
