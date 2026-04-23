using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core.Placeholders;

namespace CreativeCoders.Core.Text;

#nullable enable

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/> of <see cref="string"/>.
/// </summary>
public static class EnumerableStringExtensions
{
    /// <summary>
    /// Converts a sequence of strings containing key-value pairs into a <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    /// <param name="items">The sequence of strings to convert.</param>
    /// <param name="separator">The separator that delimits keys from values within each string.</param>
    /// <param name="ignoreInvalidEntries">
    /// <see langword="true"/> to silently skip entries that cannot be split into a key-value pair;
    /// <see langword="false"/> to throw an <see cref="ArgumentException"/> for invalid entries.
    /// </param>
    /// <returns>A dictionary containing the parsed key-value pairs.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="ignoreInvalidEntries"/> is <see langword="false"/> and an entry does not contain the separator.
    /// </exception>
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public static Dictionary<string, string> ToDictionary(this IEnumerable<string> items, string separator,
        bool ignoreInvalidEntries = true)
    {
        Ensure.NotNull(items);
        Ensure.NotNull(separator);

        return items
            .Select(x => x.SplitIntoKeyValue(separator))
            .Where(x =>
            {
                if (x == null && !ignoreInvalidEntries)
                {
                    throw new ArgumentException("Invalid key/value entry found");
                }

                return x != null;
            })
            .ToDictionary(x => x!.Key, x => x!.Value);
    }

    /// <summary>
    /// Replaces placeholders in each string of the sequence using the specified placeholder delimiters and values.
    /// </summary>
    /// <param name="items">The sequence of strings containing placeholders.</param>
    /// <param name="placeholderPrefix">The prefix that marks the start of a placeholder.</param>
    /// <param name="placeholderSuffix">The suffix that marks the end of a placeholder.</param>
    /// <param name="placeholders">The dictionary mapping placeholder names to their replacement values.</param>
    /// <returns>A sequence of strings with placeholders replaced by their corresponding values.</returns>
    [ExcludeFromCodeCoverage]
    public static IEnumerable<string> ReplacePlaceholders(this IEnumerable<string> items,
        string placeholderPrefix, string placeholderSuffix,
        IDictionary<string, object?> placeholders)
    {
        Ensure.NotNull(items);

        var replacer = new PlaceholderReplacer(placeholderPrefix, placeholderSuffix, placeholders);

        return replacer.Replace(items);
    }
}
