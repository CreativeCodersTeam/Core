using System;
using FluentAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions;

public class GuidArgumentExtensionsTests
{
    [Fact]
    public void NotEmpty_NewGuid_ReturnsArgumentWithGuid()
    {
        var guid = Guid.NewGuid();

        // Act
        var argument = Ensure.Argument(guid, nameof(guid)).NotEmpty();

        // Assert
        argument.Value
            .Should()
            .Be(guid);
    }

    [Fact]
    public void NotEmpty_EmptyGuid_ThrowsException()
    {
        var guid = Guid.Empty;

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void NotEmpty_EmptyGuidWithMessage_ThrowsException()
    {
        var guid = Guid.Empty;
        const string message = "TestMessage";

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotEmpty(message);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(guid)));
    }

    [Fact]
    public void NotEmpty_NewGuidNullable_ReturnsArgumentWithGuid()
    {
        Guid? guid = Guid.NewGuid();

        // Act
        var argument = Ensure.Argument(guid, nameof(guid)).NotEmpty();

        // Assert
        argument.Value
            .Should()
            .Be(guid);
    }

    [Fact]
    public void NotEmpty_NullGuidNullable_ThrowsException()
    {
        Guid? guid = null;

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void NotEmpty_EmptyGuidNullable_ThrowsException()
    {
        Guid? guid = Guid.Empty;

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void NotEmpty_EmptyGuidWithMessageNullable_ThrowsException()
    {
        Guid? guid = Guid.Empty;
        const string message = "TestMessage";

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotEmpty(message);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(guid)));
    }


    [Fact]
    public void NotEmpty_ArgNotNullNewGuid_ReturnsArgumentWithGuid()
    {
        var guid = Guid.NewGuid();

        // Act
        var argument = Ensure.Argument(guid, nameof(guid)).NotNull().NotEmpty();

        // Assert
        argument.Value
            .Should()
            .Be(guid);
    }

    [Fact]
    public void NotEmpty_ArgNotNullEmptyGuid_ThrowsException()
    {
        var guid = Guid.Empty;

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotNull().NotEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void NotEmpty_ArgNotNullEmptyGuidWithMessage_ThrowsException()
    {
        var guid = Guid.Empty;
        const string message = "TestMessage";

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotNull().NotEmpty(message);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(guid)));
    }

    [Fact]
    public void NotEmpty_ArgNotNullNewGuidNullable_ReturnsArgumentWithGuid()
    {
        Guid? guid = Guid.NewGuid();

        // Act
        var argument = Ensure.Argument(guid, nameof(guid)).NotNull().NotEmpty();

        // Assert
        argument.Value
            .Should()
            .Be(guid);
    }

    [Fact]
    public void NotEmpty_ArgNotNullNullGuidNullable_ThrowsException()
    {
        Guid? guid = null;

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotNull().NotEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void NotEmpty_ArgNotNullEmptyGuidNullable_ThrowsException()
    {
        Guid? guid = Guid.Empty;

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotNull().NotEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void NotEmpty_ArgNotNullEmptyGuidWithMessageNullable_ThrowsException()
    {
        Guid? guid = Guid.Empty;
        const string message = "TestMessage";

        // Act
        Action act = () => Ensure.Argument(guid, nameof(guid)).NotNull().NotEmpty(message);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(guid)));
    }
}