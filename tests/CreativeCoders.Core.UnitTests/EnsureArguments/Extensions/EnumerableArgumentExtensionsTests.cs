using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AwesomeAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.EnsureArguments.Extensions;

public class EnumerableArgumentExtensionsTests
{
    [Fact]
    public void NotNullOrEmpty_ArrayContainingItems_ReturnsArgument()
    {
        var items = new[] {1, 2, 3};

        // Act
        var argument = Ensure.Argument(items, nameof(items)).NotNullOrEmpty();

        // Assert
        argument.Value
            .Should()
            .BeSameAs(items);
    }

    [Fact]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    public void NotNullOrEmpty_EnumerableContainingItems_ReturnsArgument()
    {
        var items = new[] {1, 2, 3}.Where(_ => true);

        // Act
        var argument = Ensure.Argument(items, nameof(items)).NotNullOrEmpty();

        // Assert
        argument.Value
            .Should()
            .BeSameAs(items);
    }

    [Fact]
    public void NotNullOrEmpty_ArrayEmpty_ThrowsException()
    {
        var items = Array.Empty<int>();

        // Act
        Action act = () => Ensure.Argument(items, nameof(items)).NotNullOrEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void NotNullOrEmpty_ArrayNull_ThrowsException()
    {
        int[]? items = null;

        // Act
        Action act = () => Ensure.Argument(items, nameof(items)).NotNullOrEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void NotEmpty_ArrayContainingItems_ReturnsArgument()
    {
        var items = new[] {1, 2, 3};

        // Act
        var argument = Ensure.Argument(items, nameof(items)).NotNull().NotEmpty();

        // Assert
        argument.Value
            .Should()
            .BeSameAs(items);
    }

    [Fact]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    public void NotEmpty_EnumerableContainingItems_ReturnsArgument()
    {
        var items = new[] {1, 2, 3}.Where(_ => true);

        // Act
        var argument = Ensure.Argument(items, nameof(items)).NotNull().NotEmpty();

        // Assert
        argument.Value
            .Should()
            .BeSameAs(items);
    }

    [Fact]
    public void NotEmpty_ArrayEmpty_ThrowsException()
    {
        var items = Array.Empty<int>();

        // Act
        Action act = () => Ensure.Argument(items, nameof(items)).NotNull().NotEmpty();

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(new int[0], 0)]
    [InlineData(new[] {1, 2, 3}, 1)]
    [InlineData(new[] {1, 2, 3}, 2)]
    [InlineData(new[] {1, 2, 3}, 3)]
    public void MinCount_ItemsCountMeetsMinCount_ReturnsArgumentNotNullWithItems(int[] items,
        int minCount)
    {
        // Act
        var argument = Ensure.Argument(items, nameof(items)).NotNull().MinCount(minCount);

        // Assert
        argument.Value
            .Should()
            .HaveCount(items.Length);
    }

    [Theory]
    [InlineData(new int[0], 1)]
    [InlineData(new[] {1, 2, 3}, 4)]
    [InlineData(new[] {1, 2, 3}, 5)]
    [InlineData(new[] {1, 2, 3}, 6)]
    public void MinCount_ItemsCountNotMeetsMinCount_ThrowsException(int[] items, int minCount)
    {
        // Act
        Action act = () => Ensure.Argument(items, nameof(items)).NotNull().MinCount(minCount);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(new int[0], 1)]
    [InlineData(new[] {1, 2, 3}, 4)]
    [InlineData(new[] {1, 2, 3}, 5)]
    [InlineData(new[] {1, 2, 3}, 6)]
    public void MinCount_ItemsCountNotMeetsMinCountWithMessage_ThrowsExceptionWithMessage(
        int[] items, int minCount)
    {
        const string message = "TestMessage";

        // Act
        Action act = () => Ensure.Argument(items, nameof(items)).NotNull().MinCount(minCount, message);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(items)));
    }

    [Theory]
    [InlineData(new int[0], 0)]
    [InlineData(new[] {1, 2, 3}, 3)]
    [InlineData(new[] {1, 2, 3}, 4)]
    [InlineData(new[] {1, 2, 3}, 5)]
    public void MaxCount_ItemsCountMeetsMaxCount_ReturnsArgumentNotNullWithItems(int[] items,
        int maxCount)
    {
        // Act
        var argument = Ensure.Argument(items, nameof(items)).NotNull().MaxCount(maxCount);

        // Assert
        argument.Value
            .Should()
            .HaveCount(items.Length);
    }

    [Theory]
    [InlineData(new int[0], -1)]
    [InlineData(new[] {1, 2, 3}, 0)]
    [InlineData(new[] {1, 2, 3}, 1)]
    [InlineData(new[] {1, 2, 3}, 2)]
    public void MaxCount_ItemsCountNotMeetsMaxCount_ThrowsException(int[] items, int maxCount)
    {
        // Act
        Action act = () => Ensure.Argument(items, nameof(items)).NotNull().MaxCount(maxCount);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(new int[0], -1)]
    [InlineData(new[] {1, 2, 3}, 0)]
    [InlineData(new[] {1, 2, 3}, 1)]
    [InlineData(new[] {1, 2, 3}, 2)]
    public void MaxCount_ItemsCountNotMeetsMaxCountWithMessage_ThrowsExceptionWithMessage(
        int[] items, int maxCount)
    {
        const string message = "TestMessage";

        // Act
        Action act = () => Ensure.Argument(items, nameof(items)).NotNull().MaxCount(maxCount, message);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(items)));
    }

    [Theory]
    [InlineData(new[] {1, 2, 3}, 0, 3)]
    [InlineData(new[] {1, 2, 3}, 1, 4)]
    [InlineData(new[] {1, 2, 3}, 2, 5)]
    [InlineData(new[] {1, 2, 3}, 3, 3)]
    public void InRange_ItemsCountIsInRange_ReturnsArgumentNotNullWithItems(
        int[] items, int minCount, int maxCount)
    {
        // Act
        var argument = Ensure.Argument(items, nameof(items)).NotNull().InRange(minCount, maxCount);

        // Assert
        argument.Value
            .Should()
            .HaveCount(items.Length);
    }

    [Theory]
    [InlineData(new[] {1, 2, 3}, 4, 4)]
    [InlineData(new[] {1, 2, 3}, 4, 5)]
    [InlineData(new[] {1, 2, 3}, 1, 1)]
    [InlineData(new[] {1, 2, 3}, 1, 2)]
    [InlineData(new[] {1, 2, 3}, 0, 2)]
    public void InRange_ItemsCountIsNotInRange_ThrowsException(int[] items, int minCount, int maxCount)
    {
        // Act
        Action act = () => Ensure.Argument(items, nameof(items)).NotNull().InRange(minCount, maxCount);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(new[] {1, 2, 3}, 4, 4)]
    [InlineData(new[] {1, 2, 3}, 4, 5)]
    [InlineData(new[] {1, 2, 3}, 1, 1)]
    [InlineData(new[] {1, 2, 3}, 1, 2)]
    [InlineData(new[] {1, 2, 3}, 0, 2)]
    public void InRange_ItemsCountIsNotInRangeWithMessage_ThrowsExceptionWithMessage(
        int[] items, int minCount, int maxCount)
    {
        const string message = "TestMessage";

        // Act
        Action act = () =>
            Ensure.Argument(items, nameof(items)).NotNull().InRange(minCount, maxCount, message);

        // Assert
        act
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(ExceptionHelper.GetMessage(message, nameof(items)));
    }
}
