using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Comparing;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Collections;

public class EnumerableExtensionTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(12, 12)]
    [InlineData(0, 12, 34)]
    public void OnlySingleOrDefault(int expected, params int[] input)
    {
        var result = input.OnlySingleOrDefault();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ForEachTest()
    {
        var list = new List<TestData>
        {
            new TestData(),
            new TestData(),
            new TestData(),
            new TestData(),
            new TestData()
        };

        var enumerable = list.ToArray();

        enumerable.ForEach(item => item.IntValue1 = 20);

        var counter = list.Count(testData => testData.IntValue1 == 20);

        Assert.True(counter == 5);
    }

    [Fact]
    public void ArrayForEachTest()
    {
        var testArray = new[] { "hello", "world" } as Array;

        var list = new List<string>();

        testArray.ForEach<string>(item => list.Add(item));

        Assert.Equal(2, list.Count);
        Assert.Equal("hello", list[0]);
        Assert.Equal("world", list[1]);
    }

    [Fact]
    public void WhereNotNullTest()
    {
        var sourceList = new List<string> { "hello", null, "world" };

        var list = sourceList.WhereNotNull().ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("hello", list[0]);
        Assert.Equal("world", list[1]);
    }

    [Fact]
    public void RemoveTestPredicate()
    {
        var list = new List<string> { "hello", null, "and", "world" };
        list.Remove(item => item == null || item.Length < 5);

        Assert.Equal(2, list.Count);
        Assert.Equal("hello", list[0]);
        Assert.Equal("world", list[1]);
    }

    [Fact]
    public void RemoveTest()
    {
        var list = new List<string> { "hello", null, "and", "world" };
        var removeList = new List<string> { null, "and" };
        list.Remove(removeList);

        Assert.Equal(2, list.Count);
        Assert.Equal("hello", list[0]);
        Assert.Equal("world", list[1]);
    }

    [Fact]
    public void ForEach_WithIndex_HasCorrectIndex()
    {
        var items = Enumerable.Range(0, 10);

        items.ForEach(Assert.Equal);
    }

    [Fact]
    public void Pipe_ChangeData_DataIsChanged()
    {
        var items = Enumerable.Range(1, 10).Select(i => new TestData { IntValue1 = i });

        items.Pipe(item => item.IntValue1 *= 2)
            .ForEach((item, index) => Assert.Equal(++index * 2, item.IntValue1));
    }

    [Fact]
    public void TakeUntil_Four_HasOnlyFirstFourElements()
    {
        var items = Enumerable.Range(1, 10);

        var result = items.TakeUntil(x => x == 4).ToArray();

        Assert.Equal(4, result.Length);
        Assert.Equal(Enumerable.Range(1, 4), result);
    }

    [Fact]
    public void TakeUntil_EmptyList_ReturnsEmptyList()
    {
        var items = Array.Empty<int>();

        var result = items.TakeUntil(x => x == 4);

        Assert.Empty(result);
    }

    [Fact]
    public void SkipUntil_Four_HasAllElementsFromFiveOn()
    {
        var items = Enumerable.Range(1, 10);

        var result = items.SkipUntil(x => x == 4).ToArray();

        Assert.Equal(6, result.Length);
        Assert.Equal(Enumerable.Range(5, 6), result);
    }

    [Fact]
    public void TakeEvery_SecondElement_ResultHasAllOddElements()
    {
        var items = Enumerable.Range(0, 11);

        var result = items.TakeEvery(2).ToArray();

        Assert.Equal(6, result.Length);
        Assert.Equal(new[] { 0, 2, 4, 6, 8, 10 }, result);
    }

    [Theory]
    [InlineData("1234")]
    public void Single_ListWithSingleItem_ReturnsTrue(params string[] items)
    {
        Assert.True(items.IsSingle());
    }

    [Theory]
    [InlineData("1234", "Hello")]
    [InlineData("1234", "Hello", "World")]
    public void Single_ListWithMoreOrLessItems_ReturnsFalse(params string[] items)
    {
        Assert.False(items.IsSingle());
    }

    [Fact]
    public void Single_ListWithNoItems_ReturnsFalse()
    {
        var items = Array.Empty<string>();

        Assert.False(items.IsSingle());
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5, 6)]
    [InlineData(1, 3, 5, 7, 0, -2)]
    public void Distinct_SingleKeyNoDuplicates_ReturnsInputList(params int[] data)
    {
        var items = data.Select(i => new TestData { IntValue1 = i }).ToArray();
        var distinctItems = items.Distinct(x => x.IntValue1);

        Assert.Equal(items, distinctItems);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 2, 4, 5, 6, 6, 7 }, new[] { 1, 2, 4, 5, 6, 7 })]
    [InlineData(new[] { -2, 1, 0, 3, 5, 7, 0, -2 }, new[] { -2, 1, 0, 3, 5, 7 })]
    public void Distinct_SingleKeyDuplicates_ReturnsFilteredItems(int[] input, int[] expectedOutput)
    {
        var items = input.Select(i => new TestData { IntValue1 = i }).ToArray();
        var distinctItems = items.Distinct(x => x.IntValue1);

        var expectedItems = expectedOutput.Select(i => new TestData { IntValue1 = i }).ToArray();
        Assert.Equal(expectedItems, distinctItems, new FuncEqualityComparer<TestData, int>(x => x.IntValue1));
    }

    [Theory]
    [InlineData(new[] { 1, 1 }, new[] { 2, 2 }, new[] { 3, 3 }, new[] { 4, 4 })]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 }, new[] { 1, 3 }, new[] { 1, 4 })]
    public void Distinct_MultipleKeysNoDuplicates_ReturnsInputList(params int[][] data)
    {
        var items = data.Select(x => new TestData { IntValue1 = x[0], IntValue3 = x[1] }).ToArray();
        var distinctItems = items.Distinct(x => x.IntValue1, x => x.IntValue3).ToArray();

        Assert.Equal(items, distinctItems);
    }

    [Theory]
    [InlineData(new[] { 1, 1, 10 }, new[] { 2, 2, 10 }, new[] { 3, 3, 10 }, new[] { 4, 4, 10 },
        new[] { 4, 4, 10 },
        new[] { 2, 2, 10 },
        new[] { 1, 1, 20 }, new[] { 2, 2, 20 }, new[] { 3, 3, 20 }, new[] { 4, 4, 20 })]
    [InlineData(new[] { 1, 1, 10 }, new[] { 1, 2, 10 }, new[] { 1, 3, 10 }, new[] { 1, 4, 10 },
        new[] { 1, 2, 10 },
        new[] { 1, 3, 10 },
        new[] { 1, 1, 20 }, new[] { 1, 2, 20 }, new[] { 1, 3, 20 }, new[] { 1, 4, 20 })]
    public void Distinct_MultipleKeysDuplicates_ReturnsFilteredItems(params int[][] data)
    {
        var items = data.Where(x => x[2] == 10)
            .Select(x => new TestData { IntValue1 = x[0], IntValue3 = x[1] })
            .ToArray();
        var distinctItems = items.Distinct(x => x.IntValue1, x => x.IntValue3).ToArray();

        var expectedItems = data.Where(x => x[2] == 20)
            .Select(x => new TestData { IntValue1 = x[0], IntValue3 = x[1] })
            .ToArray();
        Assert.Equal(expectedItems, distinctItems,
            new MultiFuncEqualityComparer<TestData, int>(x => x.IntValue1, x => x.IntValue3));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 4 })]
    public void Distinct_ThreeKeysNoDuplicates_ReturnsInput(params int[][] data)
    {
        var input = data.Select(x => new TestData { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2] })
            .ToArray();

        var distinctItems = input.Distinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3).ToArray();

        Assert.Equal(input, distinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int>(x => x.IntValue1, x => x.IntValue2,
                x => x.IntValue3));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 5 })]
    public void Distinct_FourKeysNoDuplicates_ReturnsInput(params int[][] data)
    {
        var input = data.Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3] }).ToArray();

        var distinctItems = input
            .Distinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3, x => x.IntValue4)
            .ToArray();

        Assert.Equal(input, distinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int>(x => x.IntValue1, x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 6 })]
    public void Distinct_FiveKeysNoDuplicates_ReturnsInput(params int[][] data)
    {
        var input = data.Select(x => new TestData
                { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] })
            .ToArray();

        var distinctItems = input.Distinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3,
            x => x.IntValue4,
            x => x.IntValue5).ToArray();

        Assert.Equal(input, distinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int, int>(x => x.IntValue1,
                x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4, x => x.IntValue5));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 6 })]
    public void Distinct_ArrayKeysNoDuplicates_ReturnsInput(params int[][] data)
    {
        var input = data.Select(x => new TestData
                { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] })
            .ToArray();

        var distinctItems = input.Distinct<TestData, int>(x => x.IntValue1, x => x.IntValue2,
            x => x.IntValue3, x => x.IntValue4,
            x => x.IntValue5).ToArray();

        Assert.Equal(input, distinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int, int>(x => x.IntValue1,
                x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4, x => x.IntValue5));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 10 }, new[] { 1, 2, 3, 10 }, new[] { 1, 2, 3, 20 })]
    public void Distinct_ThreeKeysDuplicates_ReturnsExpected(params int[][] data)
    {
        var input = data.Where(x => x[3] == 10)
            .Select(x => new TestData { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2] }).ToArray();
        var expected = data.Where(x => x[3] == 20)
            .Select(x => new TestData { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2] }).ToArray();

        var distinctItems = input.Distinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3).ToArray();

        Assert.Equal(expected, distinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int>(x => x.IntValue1, x => x.IntValue2,
                x => x.IntValue3));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 10 }, new[] { 1, 2, 3, 4, 10 }, new[] { 1, 2, 3, 4, 20 })]
    public void Distinct_FourKeysDuplicates_ReturnsExpected(params int[][] data)
    {
        var input = data.Where(x => x[4] == 10).Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3] }).ToArray();
        var expected = data.Where(x => x[4] == 20).Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3] }).ToArray();

        var distinctItems = input
            .Distinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3, x => x.IntValue4)
            .ToArray();

        Assert.Equal(expected, distinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int>(x => x.IntValue1, x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 10 }, new[] { 1, 2, 3, 4, 5, 10 }, new[] { 1, 2, 3, 4, 5, 20 })]
    public void Distinct_FiveKeysDuplicates_ReturnsExpected(params int[][] data)
    {
        var input = data.Where(x => x[5] == 10).Select(x => new TestData
                { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] })
            .ToArray();
        var expected = data.Where(x => x[5] == 20).Select(x => new TestData
                { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] })
            .ToArray();

        var distinctItems = input.Distinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3,
            x => x.IntValue4,
            x => x.IntValue5).ToArray();

        Assert.Equal(expected, distinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int, int>(x => x.IntValue1,
                x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4, x => x.IntValue5));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 10 }, new[] { 1, 2, 3, 4, 5, 10 }, new[] { 1, 2, 3, 4, 5, 20 })]
    public void Distinct_ArrayKeysDuplicates_ReturnsExpected(params int[][] data)
    {
        var input = data.Where(x => x[5] == 10).Select(x => new TestData
                { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] })
            .ToArray();
        var expected = data.Where(x => x[5] == 20).Select(x => new TestData
                { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] })
            .ToArray();

        var distinctItems = input.Distinct<TestData, int>(x => x.IntValue1, x => x.IntValue2,
            x => x.IntValue3, x => x.IntValue4,
            x => x.IntValue5).ToArray();

        Assert.Equal(expected, distinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int, int>(x => x.IntValue1,
                x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4, x => x.IntValue5));
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5, 6, 7, 8)]
    public void NotDistinct_UniqueItems_ReturnsEmptyList(params int[] items)
    {
        var notDistinctItems = items.NotDistinct();

        Assert.Empty(notDistinctItems);
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5, 6, 7, 8)]
    public void NotDistinct_UniqueDataItems_ReturnsEmptyList(params int[] items)
    {
        var dataItems = items.Select(x => new TestData { IntValue1 = x });

        var notDistinctDataItems = dataItems.NotDistinct(x => x.IntValue1);

        Assert.Empty(notDistinctDataItems);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 3 }, new[] { 1, 1, 2, 2, 3, 3, 3 })]
    public void NotDistinct_Duplicates_ReturnNotDistinctItems(int[] input, int[] expected)
    {
        var notDistinctItems = input.NotDistinct();

        Assert.Equal(expected, notDistinctItems);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 3 }, new[] { 1, 1, 2, 2, 3, 3, 3 })]
    public void NotDistinct_DuplicateDataItems_ReturnsNotDistinctItems(int[] input, int[] expected)
    {
        var dataItems = input.Select(x => new TestData { IntValue1 = x });
        var expectedItems = expected.Select(x => new TestData { IntValue1 = x });

        var notDistinctDataItems = dataItems.NotDistinct(x => x.IntValue1);

        Assert.Equal(expectedItems, notDistinctDataItems,
            new FuncEqualityComparer<TestData, int>(x => x.IntValue1));
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5)]
    public void NotDistinct_TwoKeysNoDuplicates_ReturnsEmptyList(params int[] values)
    {
        var items = values
            .Select(x => new TestData { IntValue1 = x, IntValue2 = x });

        var notDistinctItems = items.NotDistinct(x => x.IntValue1, x => x.IntValue2);

        Assert.Empty(notDistinctItems);
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5)]
    public void NotDistinct_ThreeKeysNoDuplicates_ReturnsEmptyList(params int[] values)
    {
        var items = values
            .Select(x => new TestData { IntValue1 = x, IntValue2 = x, IntValue3 = x });

        var notDistinctItems = items.NotDistinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3);

        Assert.Empty(notDistinctItems);
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5)]
    public void NotDistinct_FourKeysNoDuplicates_ReturnsEmptyList(params int[] values)
    {
        var items = values
            .Select(x => new TestData { IntValue1 = x, IntValue2 = x, IntValue3 = x, IntValue4 = x });

        var notDistinctItems =
            items.NotDistinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3, x => x.IntValue4);

        Assert.Empty(notDistinctItems);
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5)]
    public void NotDistinct_FiveKeysNoDuplicates_ReturnsEmptyList(params int[] values)
    {
        var items = values
            .Select(x => new TestData
                { IntValue1 = x, IntValue2 = x, IntValue3 = x, IntValue4 = x, IntValue5 = x });

        var notDistinctItems = items.NotDistinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3,
            x => x.IntValue4,
            x => x.IntValue5);

        Assert.Empty(notDistinctItems);
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5)]
    public void NotDistinct_ArrayKeysNoDuplicates_ReturnsEmptyList(params int[] values)
    {
        var items = values
            .Select(x => new TestData
                { IntValue1 = x, IntValue2 = x, IntValue3 = x, IntValue4 = x, IntValue5 = x });

        var notDistinctItems = items.NotDistinct<TestData, int>(x => x.IntValue1, x => x.IntValue2,
            x => x.IntValue3, x => x.IntValue4,
            x => x.IntValue5);

        Assert.Empty(notDistinctItems);
    }

    [Theory]
    [InlineData(new[] { 1, 2, 10 }, new[] { 1, 2, 10 }, new[] { 1, 3, 10 }, new[] { 1, 2, 20 },
        new[] { 1, 2, 20 })]
    public void NotDistinct_TwoKeysDuplicates_ReturnsDuplicates(params int[][] data)
    {
        var items = data.Where(x => x[2] == 10)
            .Select(x => new TestData { IntValue1 = x[0], IntValue2 = x[1] });
        var expectedItems = data.Where(x => x[2] == 20)
            .Select(x => new TestData { IntValue1 = x[0], IntValue2 = x[1] });

        var notDistinctItems = items.NotDistinct(x => x.IntValue1, x => x.IntValue2).ToArray();

        Assert.NotEmpty(notDistinctItems);
        Assert.Equal(expectedItems, notDistinctItems,
            new MultiFuncEqualityComparer<TestData, int, int>(x => x.IntValue1, x => x.IntValue2));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 10 }, new[] { 1, 2, 3, 10 }, new[] { 1, 2, 4, 10 }, new[] { 1, 2, 3, 20 },
        new[] { 1, 2, 3, 20 })]
    public void NotDistinct_ThreeKeysDuplicates_ReturnsDuplicates(params int[][] data)
    {
        var items = data.Where(x => x[3] == 10)
            .Select(x => new TestData { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2] });
        var expectedItems = data.Where(x => x[3] == 20)
            .Select(x => new TestData { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2] });

        var notDistinctItems = items.NotDistinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3);

        Assert.Equal(expectedItems, notDistinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int>(x => x.IntValue1, x => x.IntValue2,
                x => x.IntValue3));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 10 }, new[] { 1, 2, 3, 4, 10 }, new[] { 1, 2, 3, 5, 10 },
        new[] { 1, 2, 3, 4, 20 },
        new[] { 1, 2, 3, 4, 20 })]
    public void NotDistinct_FourKeysDuplicates_ReturnsDuplicates(params int[][] data)
    {
        var items = data.Where(x => x[4] == 10).Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3] });
        var expectedItems = data.Where(x => x[4] == 20).Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3] });

        var notDistinctItems =
            items.NotDistinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3, x => x.IntValue4);

        Assert.Equal(expectedItems, notDistinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int>(x => x.IntValue1, x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 10 }, new[] { 1, 2, 3, 4, 5, 10 }, new[] { 1, 2, 3, 4, 6, 10 },
        new[] { 1, 2, 3, 4, 5, 20 }, new[] { 1, 2, 3, 4, 5, 20 })]
    public void NotDistinct_FiveKeysDuplicates_ReturnsDuplicates(params int[][] data)
    {
        var items = data.Where(x => x[5] == 10).Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] });
        var expectedItems = data.Where(x => x[5] == 20).Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] });

        var notDistinctItems = items.NotDistinct(x => x.IntValue1, x => x.IntValue2, x => x.IntValue3,
            x => x.IntValue4,
            x => x.IntValue5);

        Assert.Equal(expectedItems, notDistinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int, int>(x => x.IntValue1,
                x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4, x => x.IntValue5));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 10 }, new[] { 1, 2, 3, 4, 5, 10 }, new[] { 1, 2, 3, 4, 6, 10 },
        new[] { 1, 2, 3, 4, 5, 20 }, new[] { 1, 2, 3, 4, 5, 20 })]
    public void NotDistinct_ArrayKeysDuplicates_ReturnsDuplicates(params int[][] data)
    {
        var items = data.Where(x => x[5] == 10).Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] });
        var expectedItems = data.Where(x => x[5] == 20).Select(x => new TestData
            { IntValue1 = x[0], IntValue2 = x[1], IntValue3 = x[2], IntValue4 = x[3], IntValue5 = x[4] });

        var notDistinctItems = items.NotDistinct<TestData, int>(x => x.IntValue1, x => x.IntValue2,
            x => x.IntValue3, x => x.IntValue4,
            x => x.IntValue5);

        Assert.Equal(expectedItems, notDistinctItems,
            new MultiFuncEqualityComparer<TestData, int, int, int, int, int>(x => x.IntValue1,
                x => x.IntValue2,
                x => x.IntValue3, x => x.IntValue4, x => x.IntValue5));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 4, 6, 8, 10 }, new[] { 0, 1, 2, 3, 4, 5 })]
    public void SelectWithIndex_(int[] data, int[] expectedIndexes)
    {
        var dataItems = data.Select(x => new TestData { IntValue1 = x });

        var itemsWithIndex = dataItems.SelectWithIndex();

        Assert.Equal(expectedIndexes, itemsWithIndex.Select(x => x.Index));
    }

    [Fact]
    public void Sort_TwoKeysAscending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 2 };
        var obj2 = new TestData { IntValue1 = 2, IntValue2 = 1 };
        var obj3 = new TestData { IntValue1 = 2, IntValue2 = 2 };

        var items = new[] { obj1, obj0, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Ascending));

        Assert.Equal(new[] { obj0, obj1, obj2, obj3 }, sortedItems);
    }

    [Fact]
    public void Sort_TwoKeysDescending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 2 };
        var obj2 = new TestData { IntValue1 = 2, IntValue2 = 1 };
        var obj3 = new TestData { IntValue1 = 2, IntValue2 = 2 };

        var items = new[] { obj1, obj0, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Descending));

        Assert.Equal(new[] { obj3, obj2, obj1, obj0 }, sortedItems);
    }

    [Fact]
    public void Sort_TwoKeysFirstAscendingSecondDescending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 2 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 1 };
        var obj2 = new TestData { IntValue1 = 2, IntValue2 = 2 };
        var obj3 = new TestData { IntValue1 = 2, IntValue2 = 1 };

        var items = new[] { obj1, obj0, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Descending));

        Assert.Equal(new[] { obj0, obj1, obj2, obj3 }, sortedItems);
    }

    [Fact]
    public void Sort_TwoKeysFirstDescendingSecondAscending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 2, IntValue2 = 1 };
        var obj1 = new TestData { IntValue1 = 2, IntValue2 = 2 };
        var obj2 = new TestData { IntValue1 = 1, IntValue2 = 1 };
        var obj3 = new TestData { IntValue1 = 1, IntValue2 = 2 };

        var items = new[] { obj1, obj0, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Ascending));

        Assert.Equal(new[] { obj0, obj1, obj2, obj3 }, sortedItems);
    }

    [Fact]
    public void Sort_ThreeKeysAscending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 2 };
        var obj2 = new TestData { IntValue1 = 1, IntValue2 = 2, IntValue3 = 0 };
        var obj3 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1 };
        var obj4 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 2 };
        var obj5 = new TestData { IntValue1 = 2, IntValue2 = 2, IntValue3 = 0 };

        var items = new[] { obj5, obj1, obj4, obj0, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue3, SortOrder.Ascending));

        Assert.Equal(new[] { obj0, obj1, obj2, obj3, obj4, obj5 }, sortedItems);
    }

    [Fact]
    public void Sort_ThreeKeysDescending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 2 };
        var obj2 = new TestData { IntValue1 = 1, IntValue2 = 2, IntValue3 = 0 };
        var obj3 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1 };
        var obj4 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 2 };
        var obj5 = new TestData { IntValue1 = 2, IntValue2 = 2, IntValue3 = 0 };

        var items = new[] { obj5, obj1, obj4, obj0, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue3, SortOrder.Descending));

        Assert.Equal(new[] { obj5, obj4, obj3, obj2, obj1, obj0 }, sortedItems);
    }

    [Fact]
    public void Sort_FourKeysAscending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 2 };
        var obj2 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 2, IntValue4 = 1 };
        var obj3 = new TestData { IntValue1 = 1, IntValue2 = 2, IntValue3 = 0, IntValue4 = 1 };
        var obj4 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1 };
        var obj5 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 2 };
        var obj6 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 2, IntValue4 = 1 };
        var obj7 = new TestData { IntValue1 = 2, IntValue2 = 2, IntValue3 = 0, IntValue4 = 1 };

        var items = new[] { obj5, obj1, obj4, obj7, obj6, obj0, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue3, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue4, SortOrder.Ascending));

        Assert.Equal(new[] { obj0, obj1, obj2, obj3, obj4, obj5, obj6, obj7 }, sortedItems);
    }

    [Fact]
    public void Sort_FourKeysDescending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 2 };
        var obj2 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 2, IntValue4 = 1 };
        var obj3 = new TestData { IntValue1 = 1, IntValue2 = 2, IntValue3 = 0, IntValue4 = 1 };
        var obj4 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1 };
        var obj5 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 2 };
        var obj6 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 2, IntValue4 = 1 };
        var obj7 = new TestData { IntValue1 = 2, IntValue2 = 2, IntValue3 = 0, IntValue4 = 1 };

        var items = new[] { obj5, obj1, obj4, obj7, obj6, obj0, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue3, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue4, SortOrder.Descending));

        Assert.Equal(new[] { obj7, obj6, obj5, obj4, obj3, obj2, obj1, obj0 }, sortedItems);
    }

    [Fact]
    public void Sort_FiveKeysAscending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1, IntValue5 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1, IntValue5 = 2 };
        var obj2 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 2, IntValue5 = 1 };
        var obj3 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 2, IntValue4 = 1, IntValue5 = 1 };
        var obj4 = new TestData { IntValue1 = 1, IntValue2 = 2, IntValue3 = 0, IntValue4 = 1, IntValue5 = 1 };
        var obj5 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1, IntValue5 = 1 };
        var obj6 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1, IntValue5 = 2 };
        var obj7 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 2, IntValue5 = 1 };
        var obj8 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 2, IntValue4 = 1, IntValue5 = 1 };
        var obj9 = new TestData { IntValue1 = 2, IntValue2 = 2, IntValue3 = 0, IntValue4 = 1, IntValue5 = 1 };

        var items = new[] { obj9, obj5, obj1, obj4, obj7, obj6, obj0, obj8, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue3, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue4, SortOrder.Ascending),
            new SortFieldInfo<TestData, int>(x => x.IntValue5, SortOrder.Ascending));

        Assert.Equal(new[] { obj0, obj1, obj2, obj3, obj4, obj5, obj6, obj7, obj8, obj9 }, sortedItems);
    }

    [Fact]
    public void Sort_FiveKeysDescending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1, IntValue5 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1, IntValue5 = 2 };
        var obj2 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 1, IntValue4 = 2, IntValue5 = 1 };
        var obj3 = new TestData { IntValue1 = 1, IntValue2 = 1, IntValue3 = 2, IntValue4 = 1, IntValue5 = 1 };
        var obj4 = new TestData { IntValue1 = 1, IntValue2 = 2, IntValue3 = 0, IntValue4 = 1, IntValue5 = 1 };
        var obj5 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1, IntValue5 = 1 };
        var obj6 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 1, IntValue5 = 2 };
        var obj7 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 1, IntValue4 = 2, IntValue5 = 1 };
        var obj8 = new TestData { IntValue1 = 2, IntValue2 = 1, IntValue3 = 2, IntValue4 = 1, IntValue5 = 1 };
        var obj9 = new TestData { IntValue1 = 2, IntValue2 = 2, IntValue3 = 0, IntValue4 = 1, IntValue5 = 1 };

        var items = new[] { obj9, obj5, obj1, obj4, obj7, obj6, obj0, obj8, obj3, obj2 };

        var sortedItems = items.Sort(new SortFieldInfo<TestData, int>(x => x.IntValue1, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue2, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue3, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue4, SortOrder.Descending),
            new SortFieldInfo<TestData, int>(x => x.IntValue5, SortOrder.Descending));

        Assert.Equal(new[] { obj9, obj8, obj7, obj6, obj5, obj4, obj3, obj2, obj1, obj0 }, sortedItems);
    }

    [Fact]
    public void OrderBy_TwoKeysAscending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 2 };
        var obj2 = new TestData { IntValue1 = 2, IntValue2 = 1 };
        var obj3 = new TestData { IntValue1 = 2, IntValue2 = 2 };

        var items = new[] { obj1, obj0, obj3, obj2 };

        var sortedItems = items.OrderBy(x => x.IntValue1, x => x.IntValue2);

        Assert.Equal(new[] { obj0, obj1, obj2, obj3 }, sortedItems);
    }

    [Fact]
    public void OrderByDescending_TwoKeysDescending_SortCorrect()
    {
        var obj0 = new TestData { IntValue1 = 1, IntValue2 = 1 };
        var obj1 = new TestData { IntValue1 = 1, IntValue2 = 2 };
        var obj2 = new TestData { IntValue1 = 2, IntValue2 = 1 };
        var obj3 = new TestData { IntValue1 = 2, IntValue2 = 2 };

        var items = new[] { obj1, obj0, obj3, obj2 };

        var sortedItems = items.OrderByDescending(x => x.IntValue1, x => x.IntValue2);

        Assert.Equal(new[] { obj3, obj2, obj1, obj0 }, sortedItems);
    }

    [Fact]
    public void Choose_IntValue2FromTestData_ReturnsCorrectData()
    {
        var items = Enumerable.Range(1, 10).Select(i => new TestData { IntValue1 = i, IntValue2 = i * 2 });
        var expectedItems = new[] { 4, 8, 12, 16, 20 };

        var result = items.Choose(x => (x.IntValue1 % 2 == 0, x.IntValue2));

        Assert.Equal(expectedItems, result);
    }

    [Fact]
    public void ForEach_WithIndex_IndexIsCorrect()
    {
        var items = Enumerable.Range(0, 10) as IEnumerable;

        items.ForEach((item, index) => Assert.Equal(item, index));
    }

    [Theory]
    [InlineData(new object[] { 1, 2, 3, 4, 5 }, 5, typeof(IEnumerable<int>), typeof(int))]
    [InlineData(new object[] { "1", "2", "3", "4", "5" }, 5, typeof(IEnumerable<string>), typeof(string))]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public void OfType_ObjectEnumerableWithIntegers_ReturnsIEnumerableOfInt(IEnumerable dataArray,
        int expectedCount, Type targetType, Type itemType)
    {
        // Act
        var result = dataArray.OfType(itemType) as IEnumerable;

        // Assert
        result
            .Should()
            .NotBeNull();

        result!
            .Should()
            .BeAssignableTo(targetType);

        result!.FastCount()
            .Should()
            .Be(expectedCount);
    }

    private class TestData
    {
        public int IntValue1 { get; set; }

        public int IntValue2 { get; init; }

        public int IntValue3 { get; init; }

        public int IntValue4 { get; init; }

        public int IntValue5 { get; init; }
    }
}
