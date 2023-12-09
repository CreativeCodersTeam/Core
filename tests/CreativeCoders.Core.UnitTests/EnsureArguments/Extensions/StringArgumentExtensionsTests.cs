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
    public void HasMinLength_StringIsTooShort_ThrowsException(string text, uint minLength)
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

    [Theory]
    [InlineData("test", 5)]
    [InlineData("test", 6)]
    [InlineData("test1234qwertz", 20)]
    public void HasMinLength_NotNullStringIsToShort_ThrowsException(string text, uint minLength)
    {
        // Act
        var act = () => Ensure.Argument(text).NotNull().HasMinLength(minLength);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("test", 4)]
    [InlineData("test", 3)]
    [InlineData("test1234qwertz", 14)]
    [InlineData("test1234qwertz", 1)]
    public void HasMinLength_NotNullStringIsLonger_ReturnsArgumentNotNull(string text, uint minLength)
    {
        // Act
        var argument = Ensure.Argument(text).NotNull().HasMinLength(minLength);

        // Assert
        argument
            .Should()
            .NotBeNull()
            .And
            .BeOfType<ArgumentNotNull<string>>();
    }

    [Theory]
    [InlineData("longerText", 5)]
    [InlineData("test001", 4)]
    [InlineData("1234567890", 9)]
    public void HasMinLength_TextLengthLongerOrEqualMinLength_NoExceptionThrown(string text, uint minLength)
    {
        // Act
        Action act = () => Ensure.Argument(text).NotNull().HasMinLength(minLength);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void HasMaxLength_TextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        const string text = null;

        // Act
        var act = () => Ensure.Argument(text).HasMaxLength(5);

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("test", 4)]
    [InlineData("tes", 4)]
    [InlineData("t", 4)]
    [InlineData("test123", 10)]
    [InlineData("", 4)] // empty string scenario
    public void HasMaxLength_TextLengthBelowOrEqualMaxLength_ReturnsArgumentNotNull(string text, uint maxLength)
    {
        // Act
        var argument = Ensure.Argument(text).HasMaxLength(maxLength);

        // Assert
        argument
            .Should()
            .NotBeNull()
            .And
            .BeOfType<ArgumentNotNull<string>>();
    }

    [Theory]
    [InlineData("test", 4)]
    [InlineData("tes", 4)]
    [InlineData("t", 4)]
    [InlineData("test123", 10)]
    [InlineData("", 4)] // empty string scenario
    public void HasMaxLength_TextNotNullAndLengthBelowOrEqualMaxLength_ReturnsArgumentNotNull(string text, uint maxLength)
    {
        // Act
        var argument = Ensure.Argument(text).NotNull().HasMaxLength(maxLength);

        // Assert
        argument
            .Should()
            .NotBeNull()
            .And
            .BeOfType<ArgumentNotNull<string>>();
    }

    [Theory]
    [InlineData("test", 3)]
    [InlineData("testing", 6)]
    [InlineData("longerText", 5)]
    public void HasMaxLength_TextLengthExceedsMaxLength_ThrowsArgumentException(string text, uint maxLength)
    {
        // Act
        Action act = () => Ensure.Argument(text).NotNull().HasMaxLength(maxLength);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("longerText", 5)]
    [InlineData("test001", 4)]
    [InlineData("1234567890", 9)]
    public void HasMaxLength_TextLengthAboveMaxLength_ThrowsArgumentException(string text, uint maxLength)
    {
        // Act
        var act = () => Ensure.Argument(text).HasMaxLength(maxLength);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }
}
