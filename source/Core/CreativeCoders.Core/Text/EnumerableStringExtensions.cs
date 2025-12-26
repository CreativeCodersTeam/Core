using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core.Placeholders;

namespace CreativeCoders.Core.Text;

#nullable enable

public static class EnumerableStringExtensions
{
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
