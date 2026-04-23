using System;
using System.IO;
using CreativeCoders.Core.IO;

// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

/// <summary>
///     Provides file system validation extension methods for <see cref="Argument{T}"/>.
/// </summary>
public static class IoArgumentExtensions
{
    /// <summary>
    ///     Ensures that the argument is not <see langword="null"/> and refers to an existing file.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument as <see cref="ArgumentNotNull{T}"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">The argument value is <see langword="null"/>.</exception>
    /// <exception cref="FileNotFoundException">The file specified by the argument does not exist.</exception>
    public static ArgumentNotNull<string> FileExists(in this Argument<string> argument,
        string message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (!FileSys.File.Exists(argument.Value))
        {
            throw new FileNotFoundException(message ?? $"Argument '{argument.Name}' file not found",
                argument.Value);
        }

        return new ArgumentNotNull<string>(argument.Value, argument.Name);
    }

    /// <summary>
    ///     Ensures that the argument refers to an existing file.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="FileNotFoundException">The file specified by the argument does not exist.</exception>
    public static ref readonly ArgumentNotNull<string> FileExists(
        in this ArgumentNotNull<string> argument, string message = null)
    {
        if (!FileSys.File.Exists(argument.Value))
        {
            throw new FileNotFoundException(message ?? $"Argument '{argument.Name}' file not found",
                argument.Value);
        }

        return ref argument;
    }

    /// <summary>
    ///     Ensures that the argument is not <see langword="null"/> and refers to an existing directory.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument as <see cref="ArgumentNotNull{T}"/> for chaining.</returns>
    /// <exception cref="ArgumentNullException">The argument value is <see langword="null"/>.</exception>
    /// <exception cref="DirectoryNotFoundException">The directory specified by the argument does not exist.</exception>
    public static ArgumentNotNull<string> DirectoryExists(in this Argument<string> argument,
        string message = null)
    {
        if (argument.Value is null)
        {
            ExceptionThrower.ThrowArgumentNullException(argument.Name, message);
        }

        if (!FileSys.Directory.Exists(argument.Value))
        {
            throw new DirectoryNotFoundException(message ??
                                                 $"Argument '{argument.Name}' directory '{argument.Value}' not found");
        }

        return new ArgumentNotNull<string>(argument.Value, argument.Name);
    }

    /// <summary>
    ///     Ensures that the argument refers to an existing directory.
    /// </summary>
    /// <param name="argument">The argument to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The validated argument for chaining.</returns>
    /// <exception cref="DirectoryNotFoundException">The directory specified by the argument does not exist.</exception>
    public static ref readonly ArgumentNotNull<string> DirectoryExists(
        in this ArgumentNotNull<string> argument, string message = null)
    {
        if (!FileSys.Directory.Exists(argument.Value))
        {
            throw new DirectoryNotFoundException(message ??
                                                 $"Argument '{argument.Name}' directory '{argument.Value}' not found");
        }

        return ref argument;
    }
}
