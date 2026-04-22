using System;

// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

#nullable enable

/// <summary>
///     Provides string validation extension methods for <see cref="Argument{T}"/>.
/// </summary>
public static class StringArgumentExtensions
{
    /// <summary>
    ///     Ensures that the string argument is not <see langword="null"/> and has at least the specified minimum length.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="minLength">The minimum allowed length.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument as <see cref="ArgumentNotNull{T}"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">The argument value is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The string length is less than <paramref name="minLength"/>.</exception>
    public static ArgumentNotNull<string> HasMinLength(this Argument<string> argument, uint minLength, string? message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (argument.Value.Length < minLength)
        {
            throw new ArgumentException(message ?? $"Argument '{argument.Name}' has not the minimum length of {minLength}", argument.Name);
        }

        return new ArgumentNotNull<string>(argument.Value, argument.Name);
    }

    /// <summary>
    ///     Ensures that the string argument has at least the specified minimum length.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="minLength">The minimum allowed length.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The string length is less than <paramref name="minLength"/>.</exception>
    public static ref readonly ArgumentNotNull<string> HasMinLength(this in ArgumentNotNull<string> argument, uint minLength, string? message = null)
    {
        if (argument.Value.Length < minLength)
        {
            throw new ArgumentException(message ?? $"Argument '{argument.Name}' has not the minimum length of {minLength}", argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the string argument is not <see langword="null"/> and does not exceed the specified maximum length.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="maxLength">The maximum allowed length.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument as <see cref="ArgumentNotNull{T}"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">The argument value is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The string length exceeds <paramref name="maxLength"/>.</exception>
    public static ArgumentNotNull<string> HasMaxLength(this Argument<string> argument, uint maxLength,
        string? message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (argument.Value.Length > maxLength)
        {
            throw new ArgumentException(message ?? $"Argument '{argument.Name}' has exceeded the maximum length of {maxLength}", argument.Name);
        }

        return new ArgumentNotNull<string>(argument.Value, argument.Name);
    }

    /// <summary>
    ///     Ensures that the string argument does not exceed the specified maximum length.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="maxLength">The maximum allowed length.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The string length exceeds <paramref name="maxLength"/>.</exception>
    public static ref readonly ArgumentNotNull<string> HasMaxLength(this in ArgumentNotNull<string> argument, uint maxLength, string? message = null)
    {
        if (argument.Value.Length > maxLength)
        {
            throw new ArgumentException(message ?? $"Argument '{argument.Name}' has exceeded the maximum length of {maxLength}", argument.Name);
        }

        return ref argument;
    }
}
