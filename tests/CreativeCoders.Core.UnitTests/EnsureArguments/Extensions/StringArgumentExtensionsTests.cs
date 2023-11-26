using System;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions;

public class StringArgumentExtensionsTests
{
    [Theory]
    [InlineData("test", 5)]
    [InlineData("test", 6)]
    [InlineData("test1234qwertz", 20)]
    public void HasMinLength_StringIsToShort_ThrowsException(string text, uint minLength)
    {
        // Act
        var act = () => Ensure.Argument(text).HasMinLength(minLength);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void HasMinLength_StringNull_ThrowsException(uint minLength)
    {
        // Arrange
        const string text = null;

        // Act
        var act = () => Ensure.Argument(text).HasMinLength(minLength);

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("test", 4)]
    [InlineData("test", 3)]
    [InlineData("test1234qwertz", 14)]
    [InlineData("test1234qwertz", 1)]
    public void HasMinLength_StringIsLonger_ReturnsArgumentNotNull(string text, uint minLength)
    {
        // Act
        var argument = Ensure.Argument(text).HasMinLength(minLength);

        // Assert
        argument
            .Should()
            .NotBeNull()
            .And
            .BeOfType<ArgumentNotNull<string>>();
    }
}
