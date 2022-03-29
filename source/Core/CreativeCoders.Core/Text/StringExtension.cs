using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Text;

[PublicAPI]
public static class StringExtension
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A string extension method that converts a text to a secure string. </summary>
    ///
    /// <param name="text"> The text to convert. </param>
    ///
    /// <returns>   Text as a SecureString. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static SecureString ToSecureString(this string text)
    {
        return ToSecureString(text, false);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A string extension method that converts a text to a secure string. </summary>
    ///
    /// <param name="text">         The text to convert. </param>
    /// <param name="makeReadOnly"> True to make the SecureString read only. </param>
    ///
    /// <returns>   Text as a readonly SecureString. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static SecureString ToSecureString(this string text, bool makeReadOnly)
    {
        var result = new SecureString();

        if (text != null)
        {
            foreach (var secChar in text.ToCharArray())
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

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A SecureString extension method that converts a SecureString to a normal string.
    /// </summary>
    ///
    /// <param name="secureString"> The secureString to convert. </param>
    ///
    /// <returns>   SecureString as a string. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static string ToNormalString(this SecureString secureString)
    {
        if (secureString == null || secureString.Length == 0)
        {
            return string.Empty;
        }

        var ptr = IntPtr.Zero;
        string result;

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

    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }

    public static bool IsNotNullOrEmpty(this string text)
    {
        return !string.IsNullOrEmpty(text);
    }

    public static bool IsNullOrWhiteSpace(this string text)
    {
        return string.IsNullOrWhiteSpace(text);
    }

    public static bool IsNotNullOrWhiteSpace(this string text)
    {
        return !string.IsNullOrWhiteSpace(text);
    }

    public static StringBuilder AppendLine(this StringBuilder stringBuilder, string line, bool suppressAppend)
    {
        if (!suppressAppend)
        {
            stringBuilder.AppendLine(line);
        }

        return stringBuilder;
    }

    public static StringBuilder AppendIf(this StringBuilder stringBuilder, bool doAppend, string text)
    {
        if (doAppend)
        {
            stringBuilder.Append(text);
        }

        return stringBuilder;
    }

    public static StringBuilder AppendLineIf(this StringBuilder stringBuilder, bool doAppend, string line)
    {
        if (doAppend)
        {
            stringBuilder.AppendLine(line);
        }

        return stringBuilder;
    }

    public static string Filter(this string text, Func<char, bool> isAllowedChar)
    {
        return text.IsNullOrEmpty() ? string.Empty : string.Join("", text.Where(isAllowedChar));
    }

    public static string Filter(this string text, params char[] filteredChars)
    {
        return text.Filter(c => !filteredChars.Contains(c));
    }
}