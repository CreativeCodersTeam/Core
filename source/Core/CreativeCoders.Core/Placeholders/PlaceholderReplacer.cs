using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Placeholders;

#nullable enable

public class PlaceholderReplacer(
    string placeholderPrefix,
    string placeholderSuffix,
    IDictionary<string, object?> placeholders)
{
    private readonly string _placeholderPrefix = Ensure.IsNotNullOrWhitespace(placeholderPrefix);

    private readonly string _placeholderSuffix = Ensure.IsNotNullOrWhitespace(placeholderSuffix);

    private readonly IDictionary<string, object?> _placeholders = Ensure.NotNull(placeholders);

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

    public IEnumerable<string> Replace(IEnumerable<string> lines, bool allowNull = false)
    {
        return _placeholders.Count == 0
            ? lines
            : lines.Select(x => Replace(x, allowNull));
    }
}
