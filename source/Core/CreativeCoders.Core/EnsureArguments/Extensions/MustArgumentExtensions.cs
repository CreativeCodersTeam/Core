using System;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

/// <summary>
///     Provides generic predicate-based validation extension methods for <see cref="Argument{T}"/>
///     and <see cref="ArgumentNotNull{T}"/>.
/// </summary>
public static class MustArgumentExtensions
{
    /// <summary>
    ///     Ensures that the argument satisfies the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="predicate">The condition the argument value must satisfy.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The argument value does not satisfy <paramref name="predicate"/>.</exception>
    public static ref readonly Argument<T> Must<T>(in this Argument<T> argument, Func<T?, bool> predicate,
        string? message = null)
    {
        if (!predicate(argument.Value))
        {
            throw new ArgumentException(message ?? $"Argument must '{predicate}'", argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the argument does not satisfy the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="predicate">The condition the argument value must not satisfy.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The argument value satisfies <paramref name="predicate"/>.</exception>
    public static ref readonly Argument<T> MustNot<T>(in this Argument<T> argument, Func<T?, bool> predicate,
        string? message = null)
    {
        if (predicate(argument.Value))
        {
            throw new ArgumentException(message ?? $"Argument must not '{predicate}'", argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the non-null argument satisfies the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="predicate">The condition the argument value must satisfy.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The argument value does not satisfy <paramref name="predicate"/>.</exception>
    public static ref readonly ArgumentNotNull<T> Must<T>(in this ArgumentNotNull<T> argument,
        Func<T, bool> predicate,
        string? message = null)
    {
        if (!predicate(argument.Value))
        {
            throw new ArgumentException(message ?? $"Argument must '{predicate}'", argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the non-null argument does not satisfy the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="predicate">The condition the argument value must not satisfy.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The argument value satisfies <paramref name="predicate"/>.</exception>
    public static ref readonly ArgumentNotNull<T> MustNot<T>(in this ArgumentNotNull<T> argument,
        Func<T, bool> predicate,
        string? message = null)
    {
        if (predicate(argument.Value))
        {
            throw new ArgumentException(message ?? $"Argument must not '{predicate}'", argument.Name);
        }

        return ref argument;
    }
}
