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

/// <summary>   Static class with methods for parameter checking. </summary>
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
[PublicAPI]
public static class Ensure
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that <paramref name="value"/> is not null. </summary>
    ///
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="value">        The value to check. </param>
    /// <param name="paramName">    Name of the <paramref name="value"/> parameter. </param>
    ///
    /// <returns>   The <paramref name="value"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
    [ContractAnnotation("value: null => halt; value: notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    public static T NotNull<T>(T? value,
        [CallerArgumentExpression("value")] string paramName = "[unknown]")
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }

        return value;
    }

    [ContractAnnotation("value: null => halt")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotNull(object? value,
        [CallerArgumentExpression("value")] string paramName = "[unknown]")
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that <paramref name="value"/>  is not null. </summary>
    ///
    /// <typeparam name="T">    Type of the exception. </typeparam>
    /// <param name="value">            The value to check. </param>
    /// <param name="createException">  The <see cref="Func{T}"/>  which creates the exception that
    ///                                 gets thrown, if <paramref name="value"/>  is null. </param>
    ///-------------------------------------------------------------------------------------------------
    [ContractAnnotation("halt <= value: null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotNull<T>(object? value, Func<T> createException) where T : Exception
    {
        if (value == null)
        {
            throw createException();
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that <paramref name="value"/>  is not null or empty. </summary>
    ///
    /// <exception cref="ArgumentException">    Thrown when <paramref name="value"/>  is null or
    ///                                         empty. </exception>
    ///
    /// <param name="value">        The value to check. </param>
    /// <param name="paramName">    Name of the <paramref name="value"/>  parameter. </param>
    ///
    /// <returns>   The <paramref name="value"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
    [ContractAnnotation("halt <= value: null; value: notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string IsNotNullOrEmpty(string? value,
        [CallerArgumentExpression("value")] string paramName = "[unknown]")
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Must not be null or empty", paramName);
        }

        return value;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that <paramref name="value"/>  is not null or empty. </summary>
    ///
    /// <exception cref="ArgumentException">    Thrown when <paramref name="value"/> is null or has
    ///                                         no elements. </exception>
    ///
    /// <typeparam name="T">    Type parameter of an element. </typeparam>
    /// <param name="value">        The value to check. </param>
    /// <param name="paramName">    Name of the <paramref name="value"/>  parameter. </param>
    ///-------------------------------------------------------------------------------------------------
    [ContractAnnotation("halt <= value: null")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsNotNullOrEmpty<T>(IEnumerable<T?>? value,
        [CallerArgumentExpression("value")] string paramName = "[unknown]")
    {
        if (value == null || !value.Any())
        {
            throw new ArgumentException("Enumeration must not be null or empty", paramName);
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensure <paramref name="value"/> is not null or whitespace. </summary>
    ///
    /// <exception cref="ArgumentException">    Thrown when <paramref name="value"/> is null or
    ///                                         whitespace. </exception>
    ///
    /// <param name="value">        The value to check. </param>
    /// <param name="paramName">    Name of the <paramref name="value"/> parameter. </param>
    ///
    /// <returns>   The <paramref name="value"/>. </returns>
    ///-------------------------------------------------------------------------------------------------
    [ContractAnnotation("halt <= value: null; value: notnull => notnull")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string IsNotNullOrWhitespace(string? value,
        [CallerArgumentExpression("value")] string paramName = "[unknown]")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Must not be null or whitespace", paramName);
        }

        return value;
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that a given file exists. </summary>
    ///
    /// <exception cref="FileNotFoundException">    Thrown when the file <paramref name="fileName"/>
    ///                                              is not present. </exception>
    ///
    /// <param name="fileName"> Name of the file to check. </param>
    /// <param name="paramName">    Name of the <paramref name="fileName"/> parameter. </param>
    /// -------------------------------------------------------------------------------------------------
    public static void FileExists(string? fileName,
        [CallerArgumentExpression("fileName")] string paramName = "[unknown]")
    {
        if (!FileSys.File.Exists(fileName))
        {
            throw new FileNotFoundException(
                $"File not found. Parameter name: {paramName}",
                fileName);
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that a given directory exists. </summary>
    ///
    /// <exception cref="DirectoryNotFoundException">   Thrown when the requested directory
    ///                                                 <paramref name="directoryName"/> is not
    ///                                                 present. </exception>
    ///
    /// <param name="directoryName">    Name of the directory to check. </param>
    /// <param name="paramName">    Name of the <paramref name="directoryName"/> parameter. </param>
    ///-------------------------------------------------------------------------------------------------
    public static void DirectoryExists(string? directoryName,
        [CallerArgumentExpression("directoryName")] string paramName = "[unknown]")
    {
        if (!FileSys.Directory.Exists(directoryName))
        {
            throw new DirectoryNotFoundException($"Directory parameter '{paramName}' not found: '{directoryName}'");
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that the unique identifier is not empty. </summary>
    ///
    /// <exception cref="ArgumentException">    Thrown when <paramref name="guid"/> is empty. </exception>
    ///
    /// <param name="guid">         The guid to check. </param>
    /// <param name="paramName">    Name of the <paramref name="guid"/> parameter. </param>
    ///-------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GuidIsNotEmpty(Guid guid,
        [CallerArgumentExpression("guid")] string paramName = "[unknown]")
    {
        if (guid.Equals(Guid.Empty))
        {
            throw new ArgumentException("Guid cannot be empty", paramName);
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that <paramref name="condition"/> is true. </summary>
    ///
    /// <param name="condition">    The condition that gets checked. </param>
    /// <param name="paramName">    Name of the parameter that gets checked. </param>
    /// <param name="message">      The message for the exception. </param>
    ///-------------------------------------------------------------------------------------------------
    [ContractAnnotation("halt <= condition: false")]
    public static void That(bool condition,
        [CallerArgumentExpression("condition")] string paramName = "[unknown]",
        string message = "Assertion failed")
    {
        if (!condition)
        {
            throw new ArgumentException(message, paramName);
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Ensures that a index meets a condition. </summary>
    ///
    /// <exception cref="ArgumentOutOfRangeException">  Thrown when <paramref name="condition"/> is
    ///                                                 false, cause index not meets requirements. </exception>
    ///
    /// <param name="condition">    The condition that gets checked. </param>
    /// <param name="paramName">    Name of the parameter that gets checked. </param>
    /// <param name="message">      The message for the exception. </param>
    ///-------------------------------------------------------------------------------------------------
    [ContractAnnotation("halt <= condition: false")]
    public static void ThatRange(bool condition,
        [CallerArgumentExpression("condition")] string paramName = "[unknown]",
        string message = "Assertion failed")
    {
        if (!condition)
        {
            throw new ArgumentOutOfRangeException(paramName, message);
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Ensures that the <parmaref name="index"/> is in range between
    ///     <paramref name="startIndex"/> and <paramref name="endIndex"/>.
    /// </summary>
    ///
    /// <exception cref="ArgumentOutOfRangeException">  Thrown when index is out of range. </exception>
    ///
    /// <param name="index">        Zero-based index of the. </param>
    /// <param name="startIndex">   The start index. </param>
    /// <param name="endIndex">     The end index. </param>
    /// <param name="paramName">    Name of the <parmaref name="index"/> parameter. </param>
    ///-------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IndexIsInRange(int index, int startIndex, int endIndex,
        [CallerArgumentExpression("index")] string paramName = "[unknown]")
    {
        if (index < startIndex || index > endIndex)
        {
            throw new ArgumentOutOfRangeException(paramName, index,
                $"Index '{index}' is out of range '{startIndex}-{endIndex}'");
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Ensures that the <parmaref name="index"/> is in range of a collection with length
    ///     <paramref name="collectionLength"/>.
    /// </summary>
    ///
    /// <exception cref="ArgumentOutOfRangeException">  Thrown when the index is out of range. </exception>
    ///
    /// <param name="index">            Zero-based index of the. </param>
    /// <param name="collectionLength"> Length of the collection. </param>
    /// <param name="paramName">        Name of the <parmaref name="index"/> parameter. </param>
    ///-------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IndexIsInRange(int index, int collectionLength,
        [CallerArgumentExpression("index")] string paramName = "[unknown]")
    {
        if (index < 0 || index >= collectionLength)
        {
            throw new ArgumentOutOfRangeException(paramName, index,
                $"Index '{index}' is out of range '{0}-{collectionLength - 1}'");
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Creates an Argument{T} for fluent argument validation. </summary>
    ///
    /// <typeparam name="T">    Generic type parameter of the arguments value. </typeparam>
    /// <param name="value">        The value to check. </param>
    /// <param name="paramName">    Name of the <paramref name="value"/> parameter. </param>
    ///
    /// <returns>   An Argument&lt;T&gt; </returns>
    ///-------------------------------------------------------------------------------------------------
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Argument<T?> Argument<T>(T? value,
        [CallerArgumentExpression("value")] string paramName = "[unknown]")
    {
        return new Argument<T?>(value, paramName);
    }
}
