using System;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

/// <summary>
///     Provides <see cref="Guid"/> validation extension methods for <see cref="Argument{T}"/>.
/// </summary>
public static class GuidArgumentExtensions
{
    /// <summary>
    ///     Ensures that the <see cref="Guid"/> argument is not <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The argument value is <see cref="Guid.Empty"/>.</exception>
    public static ref readonly Argument<Guid> NotEmpty(in this Argument<Guid> argument,
        string? message = null)
    {
        if (argument.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "GUID is empty", argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the nullable <see cref="Guid"/> argument has a value and is not <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentNullException">The argument value is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The argument value is <see cref="Guid.Empty"/>.</exception>
    public static ref readonly Argument<Guid?> NotEmpty(in this Argument<Guid?> argument,
        string? message = null)
    {
        if (!argument.Value.HasValue)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (argument.Value.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "GUID is empty", argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the <see cref="Guid"/> argument is not <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The argument value is <see cref="Guid.Empty"/>.</exception>
    public static ref readonly ArgumentNotNull<Guid> NotEmpty(in this ArgumentNotNull<Guid> argument,
        string? message = null)
    {
        if (argument.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "GUID is empty", argument.Name);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the nullable <see cref="Guid"/> argument is not <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="ArgumentException">The argument value is <see cref="Guid.Empty"/>.</exception>
    public static ref readonly ArgumentNotNull<Guid?> NotEmpty(in this ArgumentNotNull<Guid?> argument,
        string? message = null)
    {
        if (argument.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "GUID is empty", argument.Name);
        }

        return ref argument;
    }
}
