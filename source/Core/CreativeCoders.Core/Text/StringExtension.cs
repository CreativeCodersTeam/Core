using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core.Text;

/// <summary>
/// Provides extension methods for <see cref="string"/> and <see cref="StringBuilder"/> operations.
/// </summary>
[PublicAPI]
public static class StringExtension
{
    /// <summary>
    /// Converts the string to a <see cref="SecureString"/>.
    /// </summary>
    /// <param name="text">The string to convert.</param>
    /// <returns>A new <see cref="SecureString"/> containing the characters of the string.</returns>
    public static SecureString ToSecureString(this string? text)
    {
        return ToSecureString(text, false);
    }

    /// <summary>
    /// Converts the string to a <see cref="SecureString"/>, optionally making it read-only.
    /// </summary>
    /// <param name="text">The string to convert.</param>
    /// <param name="makeReadOnly">
    /// <see langword="true"/> to make the resulting <see cref="SecureString"/> read-only; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>A new <see cref="SecureString"/> containing the characters of the string.</returns>
    public static SecureString ToSecureString(this string? text, bool makeReadOnly)
    {
        var result = new SecureString();

        if (text != null)
        {
            foreach (var secChar in text)
            {
                result.AppendChar(secChar);
            }
        }

        if (makeReadOnly)
        {
            result.MakeReadOnly();
        }

        return result;
    }

    /// <summary>
    /// Converts the <see cref="SecureString"/> to a plain <see cref="string"/>.
    /// </summary>
    /// <param name="secureString">The secure string to convert.</param>
    /// <returns>The plain-text representation of the secure string, or <see cref="string.Empty"/> if the secure string is empty.</returns>
    public static string? ToNormalString(this SecureString secureString)
    {
        Ensure.NotNull(secureString);

        if (secureString.Length == 0)
        {
            return string.Empty;
        }

        var ptr = IntPtr.Zero;
        string? result;

        try
        {
            ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
            result = Marshal.PtrToStringUni(ptr);
        }
        finally
        {
            if (ptr != IntPtr.Zero)
            {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        return result;
    }

    /// <summary>
    /// Determines whether the string is <see langword="null"/> or <see cref="string.Empty"/>.
    /// </summary>
    /// <param name="text">The string to test.</param>
    /// <returns><see langword="true"/> if the string is <see langword="null"/> or empty; otherwise, <see langword="false"/>.</returns>
    [ContractAnnotation("text: null => true")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? text)
    {
        return string.IsNullOrEmpty(text);
    }

    /// <summary>
    /// Determines whether the string is neither <see langword="null"/> nor <see cref="string.Empty"/>.
    /// </summary>
    /// <param name="text">The string to test.</param>
    /// <returns><see langword="true"/> if the string is not <see langword="null"/> and not empty; otherwise, <see langword="false"/>.</returns>
    [ContractAnnotation("text: notnull => true")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? text)
    {
        return !string.IsNullOrEmpty(text);
    }

    /// <summary>
    /// Determines whether the string is <see langword="null"/>, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="text">The string to test.</param>
    /// <returns><see langword="true"/> if the string is <see langword="null"/>, empty, or white-space only; otherwise, <see langword="false"/>.</returns>
    [ContractAnnotation("text: null => true")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? text)
    {
        return string.IsNullOrWhiteSpace(text);
    }

    /// <summary>
    /// Determines whether the string is neither <see langword="null"/>, empty, nor consists only of white-space characters.
    /// </summary>
    /// <param name="text">The string to test.</param>
    /// <returns><see langword="true"/> if the string contains non-white-space content; otherwise, <see langword="false"/>.</returns>
    [ContractAnnotation("text: notnull => true")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrWhiteSpace([NotNullWhen(true)] this string? text)
    {
        return !string.IsNullOrWhiteSpace(text);
    }

    /// <summary>
    /// Appends a line to the <see cref="StringBuilder"/>, unless the append is suppressed.
    /// </summary>
    /// <param name="stringBuilder">The string builder to append to.</param>
    /// <param name="line">The line to append.</param>
    /// <param name="suppressAppend">
    /// <see langword="true"/> to suppress the append operation; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>The same <see cref="StringBuilder"/> instance for chaining.</returns>
    public static StringBuilder AppendLine(this StringBuilder stringBuilder, string line, bool suppressAppend)
    {
        if (!suppressAppend)
        {
            stringBuilder.AppendLine(line);
        }

        return stringBuilder;
    }

    /// <summary>
    /// Appends the specified text to the <see cref="StringBuilder"/> if the condition is met.
    /// </summary>
    /// <param name="stringBuilder">The string builder to append to.</param>
    /// <param name="doAppend">
    /// <see langword="true"/> to perform the append; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="text">The text to append.</param>
    /// <returns>The same <see cref="StringBuilder"/> instance for chaining.</returns>
    public static StringBuilder AppendIf(this StringBuilder stringBuilder, bool doAppend, string text)
    {
        if (doAppend)
        {
            stringBuilder.Append(text);
        }

        return stringBuilder;
    }

    /// <summary>
    /// Appends the specified line followed by a line terminator to the <see cref="StringBuilder"/> if the condition is met.
    /// </summary>
    /// <param name="stringBuilder">The string builder to append to.</param>
    /// <param name="doAppend">
    /// <see langword="true"/> to perform the append; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="line">The line to append.</param>
    /// <returns>The same <see cref="StringBuilder"/> instance for chaining.</returns>
    public static StringBuilder AppendLineIf(this StringBuilder stringBuilder, bool doAppend, string line)
    {
        if (doAppend)
        {
            stringBuilder.AppendLine(line);
        }

        return stringBuilder;
    }

    /// <summary>
    /// Filters the string by retaining only characters that satisfy the predicate.
    /// </summary>
    /// <param name="text">The string to filter.</param>
    /// <param name="isAllowedChar">A function that returns <see langword="true"/> for characters to keep.</param>
    /// <returns>A new string containing only the allowed characters, or <see cref="string.Empty"/> if the input is <see langword="null"/> or empty.</returns>
    public static string Filter(this string? text, Func<char, bool> isAllowedChar)
    {
        return text.IsNullOrEmpty() ? string.Empty : string.Join("", text.Where(isAllowedChar));
    }

    /// <summary>
    /// Filters the string by removing the specified characters.
    /// </summary>
    /// <param name="text">The string to filter.</param>
    /// <param name="filteredChars">The characters to remove.</param>
    /// <returns>A new string with the specified characters removed.</returns>
    public static string Filter(this string? text, params char[] filteredChars)
    {
        return text.Filter(c => !filteredChars.Contains(c));
    }

    /// <summary>
    /// Converts a camelCase string to PascalCase by capitalizing the first character.
    /// </summary>
    /// <param name="text">The camelCase string to convert.</param>
    /// <returns>The PascalCase representation, or <see cref="string.Empty"/> if the input is <see langword="null"/> or empty.</returns>
    public static string CamelCaseToPascalCase(this string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        return char.ToUpperInvariant(text[0]) + text[1..];
    }

    /// <summary>
    /// Converts a kebab-case string to PascalCase.
    /// </summary>
    /// <param name="text">The kebab-case string to convert.</param>
    /// <returns>The PascalCase representation, or <see cref="string.Empty"/> if the input is <see langword="null"/> or empty.</returns>
    public static string KebabCaseToPascalCase(this string? text)
    {
        return SeparatedToPascalCase(text, '-');
    }

    /// <summary>
    /// Converts a snake_case string to PascalCase.
    /// </summary>
    /// <param name="text">The snake_case string to convert.</param>
    /// <returns>The PascalCase representation, or <see cref="string.Empty"/> if the input is <see langword="null"/> or empty.</returns>
    public static string SnakeCaseToPascalCase(this string? text)
    {
        return SeparatedToPascalCase(text, '_');
    }

    private static string SeparatedToPascalCase(this string? text, char separator)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var parts = text.Split(separator);

        if (parts.Length == 1)
        {
            return text.CamelCaseToPascalCase();
        }

        return parts.Aggregate(string.Empty,
            (current, part) => current + char.ToUpperInvariant(part[0]) + part[1..].ToLowerInvariant());
    }

    /// <summary>
    /// Splits the string into a key-value pair at the first occurrence of the specified separator.
    /// </summary>
    /// <param name="text">The string to split.</param>
    /// <param name="separator">The separator string to split on.</param>
    /// <returns>
    /// A <see cref="KeyAndValue"/> containing the key and value parts, or <see langword="null"/>
    /// if the input is <see langword="null"/>, empty, or does not contain the separator.
    /// </returns>
    public static KeyAndValue? SplitIntoKeyValue(this string? text, string separator)
    {
        Ensure.NotNull(separator);

        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var separatorIndex = text.IndexOf(separator, StringComparison.Ordinal);

        if (separatorIndex == -1)
        {
            return null;
        }

        var key = text[..separatorIndex];
        var value = text[(separatorIndex + 1)..];

        return new KeyAndValue(key, value);
    }
}
