using CreativeCoders.Core.Comparing;
using CreativeCoders.Core.UnitTests.Comparing.TestData;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Comparing;

public class MultiFuncComparerTests
{
    [Theory]
    [InlineData(1, 1, 1, 2, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 2, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 2, 2, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 2, 1, 3, SortOrder.Ascending, -1)]
    [InlineData(1, 2, 4, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, SortOrder.Ascending, 0)]
    [InlineData(1, 2, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(2, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(2, 1, 1, 2, SortOrder.Ascending, 1)]
    [InlineData(1, 3, 1, 2, SortOrder.Ascending, 1)]
    [InlineData(4, 1, 1, 2, SortOrder.Ascending, 1)]
    [InlineData(1, 1, 1, 2, SortOrder.Descending, 1)]
    [InlineData(1, 1, 2, 1, SortOrder.Descending, 1)]
    [InlineData(1, 2, 2, 1, SortOrder.Descending, 1)]
    [InlineData(1, 2, 1, 3, SortOrder.Descending, 1)]
    [InlineData(1, 2, 4, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, SortOrder.Descending, 0)]
    [InlineData(1, 2, 1, 1, SortOrder.Descending, -1)]
    [InlineData(2, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(2, 1, 1, 2, SortOrder.Descending, -1)]
    [InlineData(1, 3, 1, 2, SortOrder.Descending, -1)]
    [InlineData(4, 1, 1, 2, SortOrder.Descending, -1)]
    public void Compare_TwoKeys_ReturnsExpectedResult(int obj0Value1, int obj0Value2, int obj1Value1,
        int obj1Value2, SortOrder sortOrder, int expectedResult)
    {
        var obj0 = new ComparerTestObject {IntValue1 = obj0Value1, IntValue2 = obj0Value2};
        var obj1 = new ComparerTestObject {IntValue1 = obj1Value1, IntValue2 = obj1Value2};

        var comparer = new MultiFuncComparer<ComparerTestObject, int, int>(
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue1, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue2, sortOrder));

        var result = comparer.Compare(obj0, obj1);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, 1, 1, 1, 1, 2, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 2, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 2, 1, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 1, SortOrder.Ascending, 0)]
    [InlineData(1, 1, 2, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 2, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(2, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 1, 1, 1, 1, 2, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 2, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 2, 1, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, SortOrder.Descending, 0)]
    [InlineData(1, 1, 2, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(1, 2, 1, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(2, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    public void Compare_ThreeKeys_ReturnsExpectedResult(int obj0Value1, int obj0Value2, int obj0Value3,
        int obj1Value1,
        int obj1Value2, int obj1Value3, SortOrder sortOrder, int expectedResult)
    {
        var obj0 = new ComparerTestObject
            {IntValue1 = obj0Value1, IntValue2 = obj0Value2, IntValue3 = obj0Value3};
        var obj1 = new ComparerTestObject
            {IntValue1 = obj1Value1, IntValue2 = obj1Value2, IntValue3 = obj1Value3};

        var comparer = new MultiFuncComparer<ComparerTestObject, int, int, int>(
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue1, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue2, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue3, sortOrder));

        var result = comparer.Compare(obj0, obj1);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 2, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 1, 2, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 2, 1, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 2, 1, 1, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 1, SortOrder.Ascending, 0)]
    [InlineData(1, 1, 1, 2, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 1, 2, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 2, 1, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(2, 1, 1, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 2, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 2, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 2, 1, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 2, 1, 1, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 1, SortOrder.Descending, 0)]
    [InlineData(1, 1, 1, 2, 1, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(1, 1, 2, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(1, 2, 1, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(2, 1, 1, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    public void Compare_FourKeys_ReturnsExpectedResult(int obj0Value1, int obj0Value2, int obj0Value3,
        int obj0Value4,
        int obj1Value1, int obj1Value2, int obj1Value3, int obj1Value4, SortOrder sortOrder,
        int expectedResult)
    {
        var obj0 = new ComparerTestObject
            {IntValue1 = obj0Value1, IntValue2 = obj0Value2, IntValue3 = obj0Value3, IntValue4 = obj0Value4};
        var obj1 = new ComparerTestObject
            {IntValue1 = obj1Value1, IntValue2 = obj1Value2, IntValue3 = obj1Value3, IntValue4 = obj1Value4};

        var comparer = new MultiFuncComparer<ComparerTestObject, int, int, int, int>(
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue1, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue2, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue3, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue4, sortOrder));

        var result = comparer.Compare(obj0, obj1);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 2, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 2, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 2, 1, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 1, 2, 1, 1, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 2, 1, 1, 1, 1, SortOrder.Ascending, -1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, SortOrder.Ascending, 0)]
    [InlineData(1, 1, 1, 1, 2, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 1, 1, 2, 1, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 1, 2, 1, 1, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 2, 1, 1, 1, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(2, 1, 1, 1, 1, 1, 1, 1, 1, 1, SortOrder.Ascending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 2, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 2, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 2, 1, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 2, 1, 1, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 2, 1, 1, 1, 1, SortOrder.Descending, 1)]
    [InlineData(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, SortOrder.Descending, 0)]
    [InlineData(1, 1, 1, 1, 2, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(1, 1, 1, 2, 1, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(1, 1, 2, 1, 1, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(1, 2, 1, 1, 1, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    [InlineData(2, 1, 1, 1, 1, 1, 1, 1, 1, 1, SortOrder.Descending, -1)]
    public void Compare_FiveKeys_ReturnsExpectedResult(int obj0Value1, int obj0Value2, int obj0Value3,
        int obj0Value4, int obj0Value5,
        int obj1Value1, int obj1Value2, int obj1Value3, int obj1Value4, int obj1Value5, SortOrder sortOrder,
        int expectedResult)
    {
        var obj0 = new ComparerTestObject
        {
            IntValue1 = obj0Value1, IntValue2 = obj0Value2, IntValue3 = obj0Value3, IntValue4 = obj0Value4,
            IntValue5 = obj0Value5
        };
        var obj1 = new ComparerTestObject
        {
            IntValue1 = obj1Value1, IntValue2 = obj1Value2, IntValue3 = obj1Value3, IntValue4 = obj1Value4,
            IntValue5 = obj1Value5
        };

        var comparer = new MultiFuncComparer<ComparerTestObject, int, int, int, int, int>(
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue1, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue2, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue3, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue4, sortOrder),
            new SortFieldInfo<ComparerTestObject, int>(x => x.IntValue5, sortOrder));

        var result = comparer.Compare(obj0, obj1);

        Assert.Equal(expectedResult, result);
    }
}
