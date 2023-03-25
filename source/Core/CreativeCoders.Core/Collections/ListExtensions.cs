using System.Collections.Generic;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Collections;

[PublicAPI]
public static class ListExtensions
{
    public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
        Ensure.IsNotNull(items, nameof(items));

        items.ForEach(list.Add);

        return list;
    }

    public static IList<T> AddRange<T>(this IList<T> list, params T[] items)
    {
        Ensure.IsNotNull(items, nameof(items));

        items.ForEach(list.Add);

        return list;
    }

    public static void SetItems<T>(this IList<T> list, IEnumerable<T> items)
    {
        Ensure.IsNotNull(items, nameof(items));

        list.Clear();
        list.AddRange(items);
    }
}
