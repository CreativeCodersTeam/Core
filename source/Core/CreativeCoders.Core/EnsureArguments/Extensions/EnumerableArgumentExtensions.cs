using System;
using System.Collections;
using CreativeCoders.Core.Collections;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

/// <summary>
///     Provides collection validation extension methods for <see cref="Argument{T}"/>.
/// </summary>
public static class EnumerableArgumentExtensions
{
    /// <summary>
    ///     Ensures that the enumerable argument is not <see langword="null"/> and contains at least one element.
    /// </summary>
    /// <typeparam name="T">The enumerable type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument as <see cref="ArgumentNotNull{T}"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">The argument value is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The enumerable contains no elements.</exception>
    public static ArgumentNotNull<T> NotNullOrEmpty<T>(in this Argument<T> argument, string? message = null)
        where T : IEnumerable?
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        var enumerator = argument.Value.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new ArgumentException(message ?? "Argument is empty", argument.Name);
        }

        (enumerator as IDisposable)?.Dispose();

        return new ArgumentNotNull<T>(argument.Value, argument.Name);
    }

    /// <summary>
    ///     Ensures that the enumerable argument contains at least one element.
    /// </summary>
    /// <typeparam name="T">The enumerable type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The enumerable contains no elements.</exception>
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

    /// <summary>
    ///     Ensures that the enumerable argument contains at least the specified number of elements.
    /// </summary>
    /// <typeparam name="T">The enumerable type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="minCount">The minimum required number of elements.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The element count is less than <paramref name="minCount"/>.</exception>
    public static ref readonly ArgumentNotNull<T> MinCount<T>(in this ArgumentNotNull<T> argument,
        int minCount,
        string? message = null)
        where T : IEnumerable
    {
        if (argument.Value.FastCount(minCount) < minCount)
        {
            throw new ArgumentException(message ?? $"Must have length greater or equal {minCount}",
                argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the enumerable argument contains at most the specified number of elements.
    /// </summary>
    /// <typeparam name="T">The enumerable type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="maxCount">The maximum allowed number of elements.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The element count exceeds <paramref name="maxCount"/>.</exception>
    public static ref readonly ArgumentNotNull<T> MaxCount<T>(in this ArgumentNotNull<T> argument,
        int maxCount,
        string? message = null)
        where T : IEnumerable
    {
        if (argument.Value.FastCount(maxCount + 1) > maxCount)
        {
            throw new ArgumentException(message ?? $"Must have length lesser or equal {maxCount}",
                argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the enumerable argument element count is within the specified range.
    /// </summary>
    /// <typeparam name="T">The enumerable type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="minCount">The minimum required number of elements.</param>
    /// <param name="maxCount">The maximum allowed number of elements.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The element count is outside the range of <paramref name="minCount"/> to <paramref name="maxCount"/>.</exception>
    public static ref readonly ArgumentNotNull<T> InRange<T>(in this ArgumentNotNull<T> argument,
        int minCount,
        int maxCount, string? message = null)
        where T : IEnumerable
    {
        if (!argument.Value.FastCountInRange(minCount, maxCount))
        {
            throw new ArgumentException(message ?? $"Must be withing range {minCount} - {maxCount}",
                argument.Name);
        }

        return ref argument;
    }
}
