using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using CreativeCoders.Core.Text;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Text;

/// <summary>
/// Tests for <see cref="StringExtension.SplitIntoKeyValue(string,string)"/>.
/// </summary>
[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class StringExtensionSplitIntoKeyValueTests
{
    [Fact]
    public void SplitIntoKeyValue_WithSingleCharSeparator_ReturnsKeyAndValue()
    {
        // Arrange
        const string input = "User:Alice";

        // Act
        var kv = input.SplitIntoKeyValue(":");

        // Assert
        kv
            .Should()
            .NotBeNull();

        kv!.Key
            .Should()
            .Be("User");

        kv.Value
            .Should()
            .Be("Alice");
    }

    [Fact]
    public void SplitIntoKeyValue_WithMultipleSeparators_UsesFirstOccurrence()
    {
        // Arrange
        const string input = "A:B:C";

        // Act
        var kv = input.SplitIntoKeyValue(":");

        // Assert
        kv
            .Should()
            .NotBeNull();

        kv!.Key
            .Should()
            .Be("A");

        kv.Value
            .Should()
            .Be("B:C");
    }

    [Fact]
    public void SplitIntoKeyValue_WithEmptyKey_ReturnsEmptyKey()
    {
        // Arrange
        const string input = ":value";

        // Act
        var kv = input.SplitIntoKeyValue(":");

        // Assert
        kv
            .Should()
            .NotBeNull();

        kv!.Key
            .Should()
            .Be(string.Empty);

        kv.Value
            .Should()
            .Be("value");
    }

    [Fact]
    public void SplitIntoKeyValue_WithEmptyValue_ReturnsEmptyValue()
    {
        // Arrange
        const string input = "key:";

        // Act
        var kv = input.SplitIntoKeyValue(":");

        // Assert
        kv
            .Should()
            .NotBeNull();

        kv!.Key
            .Should()
            .Be("key");

        kv.Value
            .Should()
            .Be(string.Empty);
    }

    [Fact]
    public void SplitIntoKeyValue_WhenSeparatorMissing_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const string input = "NoSeparatorHere";

        // Act
        var kv = input.SplitIntoKeyValue(":");

        // Assert
        kv
            .Should()
            .BeNull();
    }

    [Fact]
    public void SplitIntoKeyValue_InputIsNull_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const string input = null;

        // Act
        var kv = input.SplitIntoKeyValue(":");

        // Assert
        kv
            .Should()
            .BeNull();
    }
}
