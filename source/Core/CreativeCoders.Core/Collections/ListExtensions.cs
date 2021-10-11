using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Collections
{
    [PublicAPI]
    public static class ListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            Ensure.IsNotNull(items, nameof(items));

            items.ForEach(list.Add);
        }

        public static void SetItems<T>(this IList<T> list, IEnumerable<T> items)
        {
            Ensure.IsNotNull(items, nameof(items));

            list.Clear();
            list.AddRange(items);
        }
    }
}