using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CreativeCoders.Core.Collections;
using AwesomeAssertions;
using Xunit;

#nullable enable
namespace CreativeCoders.Core.UnitTests.Collections;

public class CollectionExtensionsTests
{
    [Fact]
    public void TryGetCollectionCount_EmptyCollections_CountIsReadAndZero()
    {
        TestTryGetCollectionCount(Array.Empty<object>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<bool>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<byte>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<short>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<ushort>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<int>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<uint>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<long>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<ulong>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<double>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<float>(), true, 0);
        TestTryGetCollectionCount(Array.Empty<DateTime>(), true, 0);
        TestTryGetCollectionCount(string.Empty, true, 0);
        TestTryGetCollectionCount(ImmutableList<int>.Empty, true, 0);
        TestTryGetCollectionCount(new List<ulong>(), true, 0);
        TestTryGetCollectionCount(new List<uint>(), true, 0);
        TestTryGetCollectionCount(new List<ushort>(), true, 0);
    }

    [Theory]
    [InlineData(new object[] { 1, "Text" }, 2)]
    [InlineData(new[] { true, false }, 2)]
    [InlineData(new byte[] { 3, 4 }, 2)]
    [InlineData(new short[] { 5, 6 }, 2)]
    [InlineData(new ushort[] { 5, 6 }, 2)]
    [InlineData(new[] { 7, 8 }, 2)]
    [InlineData(new uint[] { 9, 10 }, 2)]
    [InlineData(new long[] { 11, 12 }, 2)]
    [InlineData(new ulong[] { 13, 14 }, 2)]
    [InlineData(new double[] { 15, 16 }, 2)]
    [InlineData(new float[] { 17, 18 }, 2)]
    [InlineData("12", 2)]
    public void TryGetCollectionCount_FilledCollections_CountIsReadAndEqualsCount(IEnumerable items,
        int expectedCount)
    {
        TestTryGetCollectionCount(items, true, expectedCount);
    }

    [Fact]
    public void TryGetCollectionCount_DateTimeFilledCollections_CountIsReadAndEqualsCount()
    {
        TestTryGetCollectionCount(new[] { DateTime.Now, DateTime.MinValue }, true, 2);
    }

    [Fact]
    public void TryGetCollectionCount_RealEnumerable_CountCanNotBeRead()
    {
        // Where clause is needed since net8, cause Enumerable.Range now returns a collection under the hood
        var items = Enumerable.Range(1, 10).Where(x => x < 100);

        TestTryGetCollectionCount(items, false, 0);
    }

    [Fact]
    public void FastCount_WithEmptyCollections_ReturnZero()
    {
        TestFastCount(Array.Empty<object>(), 0, 1);
        TestFastCount(Array.Empty<bool>(), 0, 1);
        TestFastCount(Array.Empty<byte>(), 0, 1);
        TestFastCount(Array.Empty<short>(), 0, 1);
        TestFastCount(Array.Empty<ushort>(), 0, 1);
        TestFastCount(Array.Empty<int>(), 0, 1);
        TestFastCount(Array.Empty<uint>(), 0, 1);
        TestFastCount(Array.Empty<long>(), 0, 1);
        TestFastCount(Array.Empty<ulong>(), 0, 1);
        TestFastCount(Array.Empty<double>(), 0, 1);
        TestFastCount(Array.Empty<float>(), 0, 1);
        TestFastCount(Array.Empty<DateTime>(), 0, 1);
        TestFastCount(string.Empty, 0, 1);
        TestFastCount(ImmutableList<int>.Empty, 0, 1);
        TestFastCount(new List<ulong>(), 0, 1);
        TestFastCount(new List<uint>(), 0, 1);
        TestFastCount(new List<ushort>(), 0, 1);
        TestFastCount(Array.Empty<int>().Where(_ => true), 0, 1);
    }

    [Fact]
    public void FastCount_WithFiledCollections_ReturnCount()
    {
        TestFastCount(new[] { "1234", "Test" }, 2, 0);
        TestFastCount(new[] { 1, 2 }, 2, 1);
        TestFastCount(new[] { DateTime.Now, DateTime.MinValue }, 2, 0);
        TestFastCount("12", 2, 0);
        TestFastCount(new[] { 1, 2 }.ToImmutableList(), 2, 0);
        TestFastCount(new List<ulong> { 3, 4 }, 2, 0);
        TestFastCount(new[] { 1, 2 }.Where(_ => true), 2, 0);
    }

    [Fact]
    public void FastCount_WithFiledCollectionsAndMaxCount_ReturnMaxCount()
    {
        TestFastCount(new[] { "1234", "Test", "abcd" }, 3, 2);
        TestFastCount(new[] { 1, 2, 3 }, 3, 2);
        TestFastCount(new[] { DateTime.Now, DateTime.MinValue, DateTime.MaxValue }, 3, 2);
        TestFastCount("123", 3, 2);
        TestFastCount(new[] { 1, 2, 3 }.ToImmutableList(), 3, 2);
        TestFastCount(new List<ulong> { 3, 4, 5 }, 3, 2);
        TestFastCount(new[] { 1, 2, 3 }.Where(_ => true), 2, 2);
    }

    private static void TestFastCount(IEnumerable items, int expectedCount, int maxCount)
    {
        var count = items.FastCount(maxCount);

        count
            .Should()
            .Be(expectedCount);
    }

    private static void TestTryGetCollectionCount(IEnumerable items, bool expectedCountIsRead,
        int expectedCount)
    {
        var countIsRead = items.TryGetCollectionCount(out var count);

        countIsRead
            .Should()
            .Be(expectedCountIsRead);

        count
            .Should()
            .Be(expectedCount);
    }
}
