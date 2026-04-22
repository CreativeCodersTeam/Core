using System.Text.RegularExpressions;

#nullable enable

namespace CreativeCoders.Core.Text;

/// <summary>
/// Provides wildcard pattern matching for strings using <c>*</c> and <c>?</c> placeholders.
/// </summary>
public static class PatternMatcher
{
    /// <summary>
    /// Determines whether the specified name matches the wildcard mask pattern.
    /// The mask supports <c>*</c> (zero or more characters) and <c>?</c> (exactly one character) wildcards.
    /// The comparison is case-insensitive.
    /// </summary>
    /// <param name="name">The name to match against the pattern.</param>
    /// <param name="mask">The wildcard mask pattern.</param>
    /// <returns><see langword="true"/> if the name matches the mask; otherwise, <see langword="false"/>.</returns>
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
