using System;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

/// <summary>
///     Provides null validation extension methods for <see cref="Argument{T}"/>.
/// </summary>
public static class NullArgumentExtensions
{
    /// <summary>
    ///     Ensures that the argument is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument as <see cref="ArgumentNotNull{T}"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">The argument value is <see langword="null"/>.</exception>
    public static ArgumentNotNull<T> NotNull<T>(in this Argument<T?> argument, string? message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        return new ArgumentNotNull<T>(argument.Value, argument.Name);
    }

    /// <summary>
    ///     Ensures that the argument is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated <see langword="null"/> value.</returns>
    /// <exception cref="ArgumentException">The argument value is not <see langword="null"/>.</exception>
    public static T? Null<T>(in this Argument<T?> argument, string? message = null)
    {
        if (argument.Value is not null)
        {
            throw new ArgumentException(message ?? "Argument is not null", argument.Name);
        }

        return argument.Value;
    }
}
