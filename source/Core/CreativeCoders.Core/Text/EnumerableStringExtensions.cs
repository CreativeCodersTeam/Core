using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Text;

public static class EnumerableStringExtensions
{
    public static IDictionary<string, string> ToDictionary(this IEnumerable<string> items, string separator,
        bool ignoreInvalidEntries = true)
    {
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
            .ToDictionary(x => x.Key, x => x.Value);
    }
}
