using System;
using FluentAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions;

public class NullArgumentExtensionsTests
{
    [Fact]
    public void NotNull_ValueNotNull_ReturnsArgumentNotNull()
    {
        const string testValue = "Test";

        // Act
        var argument = Ensure.Argument(testValue, nameof(testValue)).NotNull();

        // Assert
        argument
            .Should()
            .BeOfType<ArgumentNotNull<string>>();
            
        argument.Value
            .Should()
            .Be(testValue);
    }

    [Fact]
    public void NotNull_ValueIsNull_ThrowsException()
    {
        string? testValue = null;

        // Act
        Action act = () => Ensure.Argument(testValue, nameof(testValue)).NotNull();

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void NotNull_WithMessageValueIsNull_ThrowsException()
    {
        const string message = "TestMessage";
        string? testValue = null;

        // Act
        Action act = () => Ensure.Argument(testValue, nameof(testValue)).NotNull(message);

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(testValue)));
    }

    [Fact]
    public void Null_ValueNull_ReturnsNull()
    {
        const string? testValue = null;

        // Act
        var value = Ensure.Argument(testValue, nameof(testValue)).Null();

        // Assert
        value
            .Should()
            .BeNull();
    }

    [Fact]
    public void Null_ValueIsNotNull_ThrowsException()
    {
        const string testValue = "TestText";

        // Act
        Action act = () => Ensure.Argument(testValue, nameof(testValue)).Null();

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void Null_WithMessageValueIsNotNull_ThrowsException()
    {
        const string message = "TestMessage";
        const string testValue = "TestText";

        // Act
        Action act = () => Ensure.Argument(testValue, nameof(testValue)).Null(message);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(testValue)));
    }
}