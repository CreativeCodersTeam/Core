using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Collections;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Collections;

public class CollectionCounterTests
{
    [Fact]
    public void Count_FilledArray_ReturnsArrayLength()
    {
        var items = new[] {1, 2, 3};

        // Act
        var count = Count(items);

        // Assert
        count
            .Should()
            .Be(items.Length);
    }

    [Fact]
    public void Count_FilledList_ReturnsListCount()
    {
        var items = new List<int> {1, 2, 3};

        // Act
        var count = Count(items);

        // Assert
        count
            .Should()
            .Be(items.Count);
    }

    [Fact]
    public void Count_FilledEnumerable_ReturnsEnumerableCount()
    {
        var items = new List<int> {1, 2, 3};

        // Act
        var count = Count((IEnumerable) items);

        // Assert
        count
            .Should()
            .Be(items.Count);
    }

    [Theory]
    [InlineData(new[] {1, 2, 3}, 2, 3)]
    [InlineData(new[] {1, 2, 3, 4}, 0, 4)]
    [InlineData(new[] {1, 2, 3, 4, 5}, 5, 5)]
    [InlineData(new[] {1, 2, 3, 4, 5}, 6, 5)]
    public void CountMax_FilledArray_ReturnsArrayLength(int[] items, int maxCount, int expectedCount)
    {
        // Act
        var count = CountMax(items, maxCount);

        // Assert
        count
            .Should()
            .Be(expectedCount);
    }

    [Theory]
    [InlineData(new[] {1, 2, 3}, 2, 3)]
    [InlineData(new[] {1, 2, 3, 4}, 0, 4)]
    [InlineData(new[] {1, 2, 3, 4, 5}, 5, 5)]
    [InlineData(new[] {1, 2, 3, 4, 5}, 6, 5)]
    public void CountMax_FilledList_ReturnsListCount(int[] itemsArray, int maxCount, int expectedCount)
    {
        var items = itemsArray.ToList();

        // Act
        var count = CountMax(items, maxCount);

        // Assert
        count
            .Should()
            .Be(expectedCount);
    }

    [Theory]
    [InlineData(new[] {1, 2, 3}, 2, 2)]
    [InlineData(new[] {1, 2, 3, 4}, 0, 4)]
    [InlineData(new[] {1, 2, 3, 4, 5}, 5, 5)]
    [InlineData(new[] {1, 2, 3, 4, 5}, 6, 5)]
    public void CountMax_FilledEnumerable_ReturnsEnumerableCount(int[] itemsArray, int maxCount,
        int expectedCount)
    {
        var items = (IEnumerable) itemsArray.Where(_ => true);

        // Act
        var count = CountMax(items, maxCount);

        // Assert
        count
            .Should()
            .Be(expectedCount);
    }

    private static int Count<T>(T items)
        where T : IEnumerable
    {
        return CollectionCounter<T>.Count(items);
    }

    private static int CountMax<T>(T items, int maxCount)
        where T : IEnumerable
    {
        return CollectionCounter<T>.CountMax(items, maxCount);
    }
}
