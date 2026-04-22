using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Placeholders;

#nullable enable

/// <summary>
/// Replaces placeholder tokens in text with their corresponding values from a dictionary.
/// </summary>
/// <param name="placeholderPrefix">The prefix that marks the beginning of a placeholder token.</param>
/// <param name="placeholderSuffix">The suffix that marks the end of a placeholder token.</param>
/// <param name="placeholders">The dictionary mapping placeholder names to their replacement values.</param>
public class PlaceholderReplacer(
    string placeholderPrefix,
    string placeholderSuffix,
    IDictionary<string, object?> placeholders)
{
    private readonly string _placeholderPrefix = Ensure.IsNotNullOrWhitespace(placeholderPrefix);

    private readonly string _placeholderSuffix = Ensure.IsNotNullOrWhitespace(placeholderSuffix);

    private readonly IDictionary<string, object?> _placeholders = Ensure.NotNull(placeholders);

    /// <summary>
    /// Replaces all placeholder tokens in the specified text with their corresponding values.
    /// </summary>
    /// <param name="text">The text containing placeholder tokens to replace.</param>
    /// <param name="allowNull"><see langword="true"/> to replace <see langword="null"/> values with the literal string "null"; <see langword="false"/> to replace them with an empty string.</param>
    /// <returns>The text with all recognized placeholder tokens replaced by their values.</returns>
    public string Replace(string text, bool allowNull = false)
    {
        if (_placeholders.Count == 0)
        {
            return text;
        }

        return _placeholders
            .Aggregate(text,
                (current, placeholder) =>
                    current.Replace($"{_placeholderPrefix}{placeholder.Key}{_placeholderSuffix}",
                        placeholder.Value.ToStringSafe(allowNull ? "null" : string.Empty)));
    }

    /// <summary>
    /// Replaces all placeholder tokens in each line of the specified sequence with their corresponding values.
    /// </summary>
    /// <param name="lines">The sequence of text lines containing placeholder tokens to replace.</param>
    /// <param name="allowNull"><see langword="true"/> to replace <see langword="null"/> values with the literal string "null"; <see langword="false"/> to replace them with an empty string.</param>
    /// <returns>A sequence of lines with all recognized placeholder tokens replaced by their values.</returns>
    public IEnumerable<string> Replace(IEnumerable<string> lines, bool allowNull = false)
    {
        return _placeholders.Count == 0
            ? lines
            : lines.Select(x => Replace(x, allowNull));
    }
}
