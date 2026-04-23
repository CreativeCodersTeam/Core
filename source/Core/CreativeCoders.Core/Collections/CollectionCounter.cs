using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

#nullable enable
namespace CreativeCoders.Core.Collections;

/// <summary>
///     Provides pre-compiled, high-performance counting delegates for collections of type
///     <typeparamref name="T"/>. When <typeparamref name="T"/> exposes a <c>Count</c> property,
///     it is accessed directly via a compiled expression; otherwise, the sequence is enumerated.
/// </summary>
/// <typeparam name="T">The collection type, which must implement <see cref="IEnumerable"/>.</typeparam>
public static class CollectionCounter<T>
    where T : IEnumerable
{
    /// <summary>
    ///     Gets a delegate that counts the elements in a collection, stopping at the specified maximum.
    ///     The first parameter is the collection to count; the second parameter is the maximum count
    ///     (<c>0</c> means no limit).
    /// </summary>
    public static Func<T, int, int> CountMax { get; } = GetCountFunc();

    /// <summary>
    ///     Gets a delegate that counts all elements in a collection with no upper limit.
    /// </summary>
    public static Func<T, int> Count { get; } = items => CountMax(items, 0);

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

        return property?.GetMethod;
    }
}
