using System;
using System.Linq;
using System.Security;
using System.Text;
using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public static class StringExtension
    {
        public static SecureString ToSecureString(this string text)
        {
            return ToSecureString(text, false);
        }

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

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
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
}
