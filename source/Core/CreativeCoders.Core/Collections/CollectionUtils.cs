using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

#nullable enable
namespace CreativeCoders.Core.Collections
{
    public static class CollectionUtils<T>
        where T : IEnumerable
    {
        public static Func<T, int, int> CountMax = GetCountFunc();

        public static Func<T, int> Count = items => CountMax(items, 0);

        private static Func<T, int, int> GetCountFunc()
        {
            var countGetter = GetCountGetter();

            if (countGetter == null)
            {
                return (items, maxCount) => items.FastCount(maxCount);
            }

            var itemsArgument = Expression.Parameter(typeof(T), "items");
            var countCall = Expression.Call(itemsArgument, countGetter);
            var countLambda = Expression.Lambda<Func<T, int>>(countCall, itemsArgument);
            var count = countLambda.Compile();
            return (collection, _) => count(collection);
        }

        private static MethodInfo? GetCountGetter()
        {
            var property = typeof(T).GetProperty("Count");

            return property == null
                ? null
                : property.GetMethod;
        }
    }
}
