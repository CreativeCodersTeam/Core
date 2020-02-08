using CreativeCoders.Core.Comparing;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Comparing
{
    public class FuncComparerTests
    {
        [Theory]
        [InlineData(1, 2, -1)]
        [InlineData(1, 3, -1)]
        [InlineData(-1, 3, -1)]
        [InlineData(1, 1, 0)]
        [InlineData(2, 1, 1)]
        [InlineData(3, 1, 1)]
        [InlineData(3, -1, 1)]
        public void Compare_IntValue_ReturnCorrectCompareResult(int value0, int value1, int expectedResult)
        {
            var obj0 = new ComparerTestObject {IntValue1 = value0};
            var obj1 = new ComparerTestObject {IntValue1 = value1};

            var comparer = new FuncComparer<ComparerTestObject, int>(x => x.IntValue1, SortOrder.Ascending);

            var result = comparer.Compare(obj0, obj1);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("a", "b", -1)]
        [InlineData("a", "A", -1)]
        [InlineData("a", "d", -1)]
        [InlineData("ZZ", "ZZ", 0)]
        [InlineData("b", "a", 1)]
        [InlineData("A", "a", 1)]
        [InlineData("d", "a", 1)]
        [InlineData("a", "aa", -1)]
        [InlineData("aa", "Aa", -1)]
        [InlineData("abc", "def", -1)]
        [InlineData("00", "00", 0)]
        [InlineData("bb", "aa", 1)]
        [InlineData("Ab", "ab", 1)]
        [InlineData("dD", "aA", 1)]
        public void Compare_StrValue_ReturnCorrectCompareResult(string value0, string value1, int expectedResult)
        {
            var obj0 = new ComparerTestObject {StrValue = value0};
            var obj1 = new ComparerTestObject {StrValue = value1};

            var comparer = new FuncComparer<ComparerTestObject, string>(x => x.StrValue, SortOrder.Ascending);

            var result = comparer.Compare(obj0, obj1);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Compare_AllObjectsNull_ReturnsZero()
        {
            var comparer = new FuncComparer<ComparerTestObject, string>(x => x.StrValue, SortOrder.Ascending);

            var result = comparer.Compare(null, null);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_FirstObjectIsNull_ReturnsMinusOne()
        {
            var comparer = new FuncComparer<ComparerTestObject, string>(x => x.StrValue, SortOrder.Ascending);

            var result = comparer.Compare(null, new ComparerTestObject());

            Assert.Equal(-1, result);
        }

        [Fact]
        public void Compare_SecondObjectIsNull_ReturnsOne()
        {
            var comparer = new FuncComparer<ComparerTestObject, string>(x => x.StrValue, SortOrder.Ascending);

            var result = comparer.Compare(new ComparerTestObject(), null);

            Assert.Equal(1, result);
        }
    }
}