using System.Text.RegularExpressions;

namespace CreativeCoders.Core
{
    public static class PatternMatcher
    {
        public static bool MatchesPattern(string name, string mask)
        {
            Ensure.IsNotNullOrEmpty(name, "name");
            Ensure.IsNotNullOrEmpty(mask, "mask");
            var pattern = '^' +
                          Regex.Escape(mask.Replace(".", "__DOT__").Replace("*", "__STAR__").Replace("?", "__QM__"))
                              .Replace("__DOT__", "[.]")
                              .Replace("__STAR__", ".*")
                              .Replace("__QM__", ".") + '$';
            return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(name);
        }
    }
}