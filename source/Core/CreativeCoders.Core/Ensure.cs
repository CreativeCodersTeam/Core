using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using CreativeCoders.Core.IO;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

/// <summary>
/// Provides static methods for validating method arguments and preconditions.
/// </summary>
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
[PublicAPI]
public static class Ensure
{
    private const string UnknownParamName = "[unknown]";

    /// <summary>
    /// Ensures that the specified <paramref name="value"/> is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <returns>The validated non-null value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    [ContractAnnotation("value: null => halt; value: notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    public static T NotNull<T>([System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration] T? value,
        [CallerArgumentExpression("value")] string paramName = UnknownParamName)
    {
        return value ?? throw new ArgumentNullException(paramName);
    }

    /// <summary>
    /// Ensures that the specified <paramref name="value"/> is not <see langword="null"/>.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    [ContractAnnotation("value: null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotNull([System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration] object? value,
        [CallerArgumentExpression("value")] string paramName = UnknownParamName)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    /// <summary>
    /// Ensures that the specified <paramref name="value"/> is not <see langword="null"/>, throwing a custom exception.
    /// </summary>
    /// <typeparam name="T">The type of the exception to throw.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="createException">The factory that creates the exception to throw when <paramref name="value"/> is <see langword="null"/>.</param>
    [ContractAnnotation("halt <= value: null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotNull<T>([System.Diagnostics.CodeAnalysis.NotNull] [NoEnumeration] object? value,
        Func<T> createException) where T : Exception
    {
        if (value == null)
        {
            throw createException();
        }
    }

    /// <summary>
    /// Ensures that the specified string <paramref name="value"/> is not <see langword="null"/> or empty.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <returns>The validated non-null and non-empty string.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see langword="null"/> or empty.</exception>
    [ContractAnnotation("halt <= value: null; value: notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string IsNotNullOrEmpty([System.Diagnostics.CodeAnalysis.NotNull] string? value,
        [CallerArgumentExpression("value")] string paramName = UnknownParamName)
    {
        return string.IsNullOrEmpty(value)
            ? throw new ArgumentException("Must not be null or empty", paramName)
            : value;
    }

    /// <summary>
    /// Ensures that the specified enumerable <paramref name="value"/> is not <see langword="null"/> or empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="value">The enumerable value to validate.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see langword="null"/> or has no elements.</exception>
    [ContractAnnotation("halt <= value: null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotNullOrEmpty<T>(IEnumerable<T?>? value,
        [CallerArgumentExpression("value")] string paramName = UnknownParamName)
    {
        if (value == null || !value.Any())
        {
            throw new ArgumentException("Enumeration must not be null or empty", paramName);
        }
    }

    /// <summary>
    /// Ensures that the specified string <paramref name="value"/> is not <see langword="null"/> or whitespace.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <returns>The validated non-null and non-whitespace string.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is <see langword="null"/> or whitespace.</exception>
    [ContractAnnotation("halt <= value: null; value: notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string IsNotNullOrWhitespace(string? value,
        [CallerArgumentExpression("value")] string paramName = UnknownParamName)
    {
        return string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Must not be null or whitespace", paramName)
            : value;
    }

    /// <summary>
    /// Ensures that the specified file exists.
    /// </summary>
    /// <param name="fileName">The path of the file to check.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <exception cref="FileNotFoundException"><paramref name="fileName"/> does not exist.</exception>
    public static void FileExists(string? fileName,
        [CallerArgumentExpression("fileName")] string paramName = UnknownParamName)
    {
        if (!FileSys.File.Exists(fileName))
        {
            throw new FileNotFoundException(
                $"File not found. Parameter name: {paramName}",
                fileName);
        }
    }

    /// <summary>
    /// Ensures that the specified directory exists.
    /// </summary>
    /// <param name="directoryName">The path of the directory to check.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <exception cref="DirectoryNotFoundException"><paramref name="directoryName"/> does not exist.</exception>
    public static void DirectoryExists(string? directoryName,
        [CallerArgumentExpression("directoryName")]
        string paramName = UnknownParamName)
    {
        if (!FileSys.Directory.Exists(directoryName))
        {
            throw new DirectoryNotFoundException(
                $"Directory parameter '{paramName}' not found: '{directoryName}'");
        }
    }

    /// <summary>
    /// Ensures that the specified <paramref name="guid"/> is not <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> value to validate.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <exception cref="ArgumentException"><paramref name="guid"/> is <see cref="Guid.Empty"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GuidIsNotEmpty(Guid guid,
        [CallerArgumentExpression("guid")] string paramName = UnknownParamName)
    {
        if (guid.Equals(Guid.Empty))
        {
            throw new ArgumentException("Guid cannot be empty", paramName);
        }
    }

    /// <summary>
    /// Ensures that the specified <paramref name="condition"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="message">The exception message.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <exception cref="ArgumentException"><paramref name="condition"/> is <see langword="false"/>.</exception>
    [ContractAnnotation("halt <= condition: false")]
    public static void That(bool condition,
        string message = "Assertion failed",
        [CallerArgumentExpression("condition")]
        string paramName = UnknownParamName)
    {
        if (!condition)
        {
            throw new ArgumentException(message, paramName);
        }
    }

    /// <summary>
    /// Ensures that the specified range <paramref name="condition"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="condition">The range condition to evaluate.</param>
    /// <param name="message">The exception message.</param>
    /// <param name="paramName">The name of the parameter being validated.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="condition"/> is <see langword="false"/>.</exception>
    [ContractAnnotation("halt <= condition: false")]
    public static void ThatRange(bool condition,
        string message = "Assertion failed",
        [CallerArgumentExpression("condition")]
        string paramName = UnknownParamName)
    {
        if (!condition)
        {
            throw new ArgumentOutOfRangeException(paramName, message);
        }
    }

    /// <summary>
    /// Ensures that the specified <paramref name="index"/> is in range between
    /// <paramref name="startIndex"/> and <paramref name="endIndex"/>.
    /// </summary>
    /// <param name="index">The zero-based index to validate.</param>
    /// <param name="startIndex">The inclusive lower bound of the valid range.</param>
    /// <param name="endIndex">The inclusive upper bound of the valid range.</param>
    /// <param name="paramName">The name of the <paramref name="index"/> parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of <paramref name="startIndex"/> to <paramref name="endIndex"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IndexIsInRange(int index, int startIndex, int endIndex,
        [CallerArgumentExpression("index")] string paramName = UnknownParamName)
    {
        if (index < startIndex || index > endIndex)
        {
            throw new ArgumentOutOfRangeException(paramName, index,
                $"Index '{index}' is out of range '{startIndex}-{endIndex}'");
        }
    }

    /// <summary>
    /// Ensures that the specified <paramref name="index"/> is in range for a collection with the given
    /// <paramref name="collectionLength"/>.
    /// </summary>
    /// <param name="index">The zero-based index to validate.</param>
    /// <param name="collectionLength">The length of the collection.</param>
    /// <param name="paramName">The name of the <paramref name="index"/> parameter.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the valid range for the collection.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IndexIsInRange(int index, int collectionLength,
        [CallerArgumentExpression("index")] string paramName = UnknownParamName)
    {
        if (index < 0 || index >= collectionLength)
        {
            throw new ArgumentOutOfRangeException(paramName, index,
                $"Index '{index}' is out of range '{0}-{collectionLength - 1}'");
        }
    }

    /// <summary>
    /// Creates an <see cref="Argument{T}"/> for fluent argument validation.
    /// </summary>
    /// <typeparam name="T">The type of the argument value.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The name of the <paramref name="value"/> parameter.</param>
    /// <returns>An <see cref="Argument{T}"/> wrapping the specified value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Argument<T?> Argument<T>(T? value,
        [CallerArgumentExpression("value")] string paramName = UnknownParamName)
    {
        return new Argument<T?>(value, paramName);
    }
}
