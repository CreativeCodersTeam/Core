using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AwesomeAssertions;
using CreativeCoders.Core.Placeholders;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Placeholders;

/// <summary>
/// Tests for <see cref="PlaceholderReplacer"/> to verify placeholder replacement for single strings
/// and sequences, as well as guard clauses and edge cases.
/// </summary>
[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class PlaceholderReplacerTests
{
    [Fact]
    public void Replace_String_ReplacesAllOccurrencesOfAllPlaceholders()
    {
        // Arrange
        var placeholders = new Dictionary<string, string>
        {
            ["Name"] = "Alice",
            ["City"] = "Berlin"
        };

        var replacer = new PlaceholderReplacer("${", "}", placeholders);
        const string input = "Hello ${Name}, welcome to ${City}. ${Name} likes ${City}.";

        // Act
        var result = replacer.Replace(input);

        // Assert
        result
            .Should()
            .Be("Hello Alice, welcome to Berlin. Alice likes Berlin.");
    }

    [Fact]
    public void Replace_String_WithNoMatchingPlaceholders_ReturnsOriginalString()
    {
        // Arrange
        var placeholders = new Dictionary<string, string>
        {
            ["User"] = "Bob"
        };
        var replacer = new PlaceholderReplacer("<%", "%>", placeholders);
        const string input = "Hello ${User}"; // token style does not match

        // Act
        var result = replacer.Replace(input);

        // Assert
        result
            .Should()
            .Be(input);
    }

    [Fact]
    public void Replace_String_WithEmptyPlaceholderDictionary_ReturnsSameReference()
    {
        // Arrange
        var replacer = new PlaceholderReplacer("${", "}", new Dictionary<string, string>());
        var input = "Keep me as is";

        // Act
        var result = replacer.Replace(input);

        // Assert
        ReferenceEquals(result, input)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Replace_Lines_ReplacesAcrossAllLines()
    {
        // Arrange
        var placeholders = new Dictionary<string, string>
        {
            ["Env"] = "Prod",
            ["Ver"] = "1.2.3"
        };
        var replacer = new PlaceholderReplacer("${", "}", placeholders);
        var lines = new[]
        {
            "Environment: ${Env}",
            "Version: ${Ver}",
            "${Env}-${Ver}"
        };

        // Act
        var result = replacer.Replace(lines).ToArray();

        // Assert
        result.Length
            .Should()
            .Be(3);

        result
            .Should()
            .BeEquivalentTo("Environment: Prod", "Version: 1.2.3", "Prod-1.2.3");
    }

    [Fact]
    public void Replace_Lines_WithEmptyPlaceholderDictionary_ReturnsSameEnumerableReference()
    {
        // Arrange
        var lines = new[] { "a", "b" };
        var replacer = new PlaceholderReplacer("${", "}", new Dictionary<string, string>());

        // Act
        var result = replacer.Replace(lines);

        // Assert
        ReferenceEquals(result, lines)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Constructor_WithNullPlaceholders_Throws()
    {
        // Act
        var act = () => new PlaceholderReplacer("${", "}", null!);

        // Assert
        act
            .Should()
            .ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithInvalidPrefixOrSuffix_Throws()
    {
        // Arrange
        var placeholders = new Dictionary<string, string>();

        // Act
        var actPrefixNull = () => new PlaceholderReplacer(null!, "}", placeholders);
        var actPrefixEmpty = () => new PlaceholderReplacer(string.Empty, "}", placeholders);
        var actPrefixWs = () => new PlaceholderReplacer(" ", "}", placeholders);

        var actSuffixNull = () => new PlaceholderReplacer("${", null!, placeholders);
        var actSuffixEmpty = () => new PlaceholderReplacer("${", string.Empty, placeholders);
        var actSuffixWs = () => new PlaceholderReplacer("${", " ", placeholders);

        // Assert
        actPrefixNull.Should().ThrowExactly<ArgumentException>();
        actPrefixEmpty.Should().ThrowExactly<ArgumentException>();
        actPrefixWs.Should().ThrowExactly<ArgumentException>();

        actSuffixNull.Should().ThrowExactly<ArgumentException>();
        actSuffixEmpty.Should().ThrowExactly<ArgumentException>();
        actSuffixWs.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void Replace_WithOverlappingPlaceholderNames_ReplacesExactTokensOnly()
    {
        // Arrange
        var placeholders = new Dictionary<string, string>
        {
            ["A"] = "x",
            ["AB"] = "y"
        };
        var replacer = new PlaceholderReplacer("${", "}", placeholders);
        const string input = "${A}-${AB}-${A}${AB}";

        // Act
        var result = replacer.Replace(input);

        // Assert
        result
            .Should()
            .Be("x-y-xy");
    }
}
