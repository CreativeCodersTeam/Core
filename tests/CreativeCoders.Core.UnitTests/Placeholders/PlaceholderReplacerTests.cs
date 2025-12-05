using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AwesomeAssertions;
using CreativeCoders.Core.Placeholders;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Placeholders;

#nullable enable

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
        var placeholders = new Dictionary<string, object?>
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
    public void Replace_String_ReplacesAllOccurrencesOfAllPlaceholdersWithDifferentTypes()
    {
        // Arrange
        var placeholders = new Dictionary<string, object?>
        {
            ["Name"] = "Alice",
            ["City"] = "Berlin",
            ["Age"] = 25,
            ["BigCity"] = true
        };

        var replacer = new PlaceholderReplacer("${", "}", placeholders);
        const string input =
            "Hello ${Name}, welcome to ${City}. ${Name} likes ${City}. You are ${Age} years old. ${City} is a big city = ${BigCity}.";

        // Act
        var result = replacer.Replace(input);

        // Assert
        result
            .Should()
            .Be(
                "Hello Alice, welcome to Berlin. Alice likes Berlin. You are 25 years old. Berlin is a big city = True.");
    }

    [Theory]
    [InlineData("NullName", null, true, "null")]
    [InlineData("EmptyName", null, false, "")]
    public void Replace_String_WithNullVar_AllowNullValuesLeadsToCorrectResult(string placeholderName,
        object? placeholderValue, bool allowNullValues, string expectedResult)
    {
        // Arrange
        var placeholders = new Dictionary<string, object?>
        {
            [placeholderName] = placeholderValue
        };

        var replacer = new PlaceholderReplacer("${", "}", placeholders);
        var inputText = "${" + placeholderName + "}";

        // Act
        var result = replacer.Replace(inputText, allowNullValues);

        // Assert
        result
            .Should()
            .Be(expectedResult);
    }

    [Fact]
    public void Replace_String_WithNoMatchingPlaceholders_ReturnsOriginalString()
    {
        // Arrange
        var placeholders = new Dictionary<string, object?>
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
        var replacer = new PlaceholderReplacer("${", "}", new Dictionary<string, object?>());
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
        var placeholders = new Dictionary<string, object?>
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
        var replacer = new PlaceholderReplacer("${", "}", new Dictionary<string, object?>());

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
        var placeholders = new Dictionary<string, object?>();

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
        var placeholders = new Dictionary<string, object?>
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
