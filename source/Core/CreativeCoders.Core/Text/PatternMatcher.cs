using System.Text.RegularExpressions;

#nullable enable

namespace CreativeCoders.Core.Text;

public static class PatternMatcher
{
    public static bool MatchesPattern(string name, string mask)
    {
        Ensure.IsNotNullOrEmpty(name, nameof(name));
        Ensure.IsNotNullOrEmpty(mask, nameof(mask));

        var pattern = '^' +
                      Regex.Escape(mask
                              .Replace(".", "__DOT__")
                              .Replace("*", "__STAR__")
                              .Replace("?", "__QM__"))
                          .Replace("__DOT__", "[.]")
                          .Replace("__STAR__", ".*")
                          .Replace("__QM__", ".") + '$';

        return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(name);
    }
}
