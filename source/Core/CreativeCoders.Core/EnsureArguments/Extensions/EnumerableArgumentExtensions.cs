using System;
using System.Collections;
using CreativeCoders.Core.Collections;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

public static class EnumerableArgumentExtensions
{
    public static ArgumentNotNull<T> NotNullOrEmpty<T>(in this Argument<T> argument, string? message = null)
        where T : IEnumerable?
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (!argument.Value.GetEnumerator().MoveNext())
        {
            throw new ArgumentException(message ?? "Argument is empty", argument.Name);
        }

        return new ArgumentNotNull<T>(argument.Value, argument.Name);
    }

    public static ref readonly ArgumentNotNull<T> NotEmpty<T>(in this ArgumentNotNull<T> argument,
        string? message = null)
        where T : IEnumerable
    {
        if (argument.Value.FastEmpty())
        {
            throw new ArgumentException(message ?? "Argument is empty", argument.Name);
        }

        return ref argument;
    }

    public static ref readonly ArgumentNotNull<T> MinCount<T>(in this ArgumentNotNull<T> argument, int minCount,
        string? message = null)
        where T : IEnumerable
    {
        if (argument.Value.FastCount(minCount) < minCount)
        {
            throw new ArgumentException(message ?? $"Must have length greater or equal {minCount}", argument.Name);
        }

        return ref argument;
    }

    public static ref readonly ArgumentNotNull<T> MaxCount<T>(in this ArgumentNotNull<T> argument, int maxCount,
        string? message = null)
        where T : IEnumerable
    {
        if (argument.Value.FastCount(maxCount + 1) > maxCount)
        {
            throw new ArgumentException(message ?? $"Must have length lesser or equal {maxCount}", argument.Name);
        }

        return ref argument;
    }

    public static ref readonly ArgumentNotNull<T> InRange<T>(in this ArgumentNotNull<T> argument, int minCount,
        int maxCount, string? message = null)
        where T : IEnumerable
    {
        if (!argument.Value.FastCountInRange(minCount, maxCount))
        {
            throw new ArgumentException(message ?? $"Must be withing range {minCount} - {maxCount}", argument.Name);
        }

        return ref argument;
    }
}