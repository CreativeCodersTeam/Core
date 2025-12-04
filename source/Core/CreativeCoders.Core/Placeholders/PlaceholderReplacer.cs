using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Placeholders;

public class PlaceholderReplacer(
    string placeholderPrefix,
    string placeholderSuffix,
    IDictionary<string, string> placeholders)
{
    private readonly string _placeholderPrefix = Ensure.IsNotNullOrWhitespace(placeholderPrefix);

    private readonly string _placeholderSuffix = Ensure.IsNotNullOrWhitespace(placeholderSuffix);

    private readonly IDictionary<string, string> _placeholders = Ensure.NotNull(placeholders);

    public string Replace(string text)
    {
        if (_placeholders.Count == 0)
        {
            return text;
        }

        return _placeholders
            .Aggregate(text,
                (current, placeholder) =>
                    current.Replace($"{_placeholderPrefix}{placeholder.Key}{_placeholderSuffix}",
                        placeholder.Value));
    }

    public IEnumerable<string> Replace(IEnumerable<string> lines)
    {
        return _placeholders.Count == 0
            ? lines
            : lines.Select(Replace);
    }
}
