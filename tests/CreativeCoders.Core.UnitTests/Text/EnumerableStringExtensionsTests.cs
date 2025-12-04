using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using CreativeCoders.Core.Text;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Text;

/// <summary>
/// Tests for <see cref="EnumerableStringExtensions"/> verifying conversion of string sequences
/// into dictionaries using a key/value separator.
/// </summary>
public class EnumerableStringExtensionsTests
{
    [Fact]
    public void ToDictionary_WithValidItems_ReturnsExpectedDictionary()
    {
        // Arrange
        var items = new[] { "A:1", "B:2", "C:3" };

        // Act
        var result = items.ToDictionary(":");

        // Assert
        result
            .Should()
            .HaveCount(3);

        result["A"].Should().Be("1");
        result["B"].Should().Be("2");
        result["C"].Should().Be("3");
    }

    [Fact]
    public void ToDictionary_WithDuplicateKeys_ThrowsArgumentException()
    {
        // Arrange
        var items = new[] { "Key:1", "Key:2" };

        // Act
        var act = () => items.ToDictionary(":");

        // Assert
        act
            .Should()
            .ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void ToDictionary_ItemWithoutSeparator_ReturnsOnlyValidItems()
    {
        // Arrange
        var items = new[] { "Key:1", "Invalid" };

        // Act
        var dict = items.ToDictionary(":", true);

        // Assert
        dict
            .Should()
            .HaveCount(1);

        dict["Key"]
            .Should()
            .Be("1");
    }

    [Fact]
    public void ToDictionary_ItemWithoutSeparator_ThrowsArgumentException()
    {
        // Arrange
        var items = new[] { "Key:1", "Invalid" };

        // Act
        var act = () => items.ToDictionary(":", false);

        // Assert
        act
            .Should()
            .ThrowExactly<ArgumentException>();
    }
}
