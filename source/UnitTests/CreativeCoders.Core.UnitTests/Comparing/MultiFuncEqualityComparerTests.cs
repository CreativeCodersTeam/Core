using System.Linq;
using CreativeCoders.Core.Comparing;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Comparing
{
    public class MultiFuncEqualityComparerTests
    {
        [Fact]
        public void Equals_SingleKeyNotEquals_ReturnsFalse()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int>(x => x.IntValue1);

            var testObj1 = new ComparerTestObject {IntValue1 = 1};
            var testObj2 = new ComparerTestObject {IntValue1 = 2};

            Assert.False(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_SingleKeyEquals_ReturnsTrue()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int>(x => x.IntValue1);

            var testObj1 = new ComparerTestObject {IntValue1 = 1};
            var testObj2 = new ComparerTestObject {IntValue1 = 1};

            Assert.True(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_TwoKeysSameTypeNotEquals_ReturnsFalse()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int>(x => x.IntValue1, x => x.IntValue2);

            var testObj1 = new ComparerTestObject {IntValue1 = 1, IntValue2 = 1};
            var testObj2 = new ComparerTestObject {IntValue1 = 2, IntValue2 = 1};

            Assert.False(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_TwoKeysSameTypeEquals_ReturnsTrue()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int>(x => x.IntValue1, x => x.IntValue2);

            var testObj1 = new ComparerTestObject {IntValue1 = 1, IntValue2 = 1};
            var testObj2 = new ComparerTestObject {IntValue1 = 1, IntValue2 = 1};

            Assert.True(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_TwoKeysDifferentTypeNotEquals_ReturnsFalse()
        {
            var comparer =
                new MultiFuncEqualityComparer<ComparerTestObject, int, string>(x => x.IntValue1, x => x.StrValue);

            var testObj1 = new ComparerTestObject {IntValue1 = 1, StrValue = "Hello"};
            var testObj2 = new ComparerTestObject {IntValue1 = 2, StrValue = "Hello"};

            Assert.False(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_TwoKeysDifferentTypeEquals_ReturnsTrue()
        {
            var comparer =
                new MultiFuncEqualityComparer<ComparerTestObject, int, string>(x => x.IntValue1, x => x.StrValue);

            var testObj1 = new ComparerTestObject {IntValue1 = 1, StrValue = "Hello"};
            var testObj2 = new ComparerTestObject {IntValue1 = 1, StrValue = "Hello"};

            Assert.True(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_ThreeKeysDifferentTypeNotEquals_ReturnsFalse()
        {
            var comparer =
                new MultiFuncEqualityComparer<ComparerTestObject, int, string, int>(x => x.IntValue1, x => x.StrValue,
                    x => x.IntValue2);

            var testObj1 = new ComparerTestObject {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3};
            var testObj2 = new ComparerTestObject {IntValue1 = 2, StrValue = "Hello", IntValue2 = 3};

            Assert.False(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_ThreeKeysDifferentTypeEquals_ReturnsTrue()
        {
            var comparer =
                new MultiFuncEqualityComparer<ComparerTestObject, int, string, int>(x => x.IntValue1, x => x.StrValue,
                    x => x.IntValue2);

            var testObj1 = new ComparerTestObject {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3};
            var testObj2 = new ComparerTestObject {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3};

            Assert.True(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_FourKeysDifferentTypeNotEquals_ReturnsFalse()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int, string, int, string>(x => x.IntValue1,
                x => x.StrValue, x => x.IntValue2, x => x.StrValue1);

            var testObj1 = new ComparerTestObject
                {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3, StrValue1 = "World"};
            var testObj2 = new ComparerTestObject
                {IntValue1 = 2, StrValue = "Hello", IntValue2 = 3, StrValue1 = "World"};

            Assert.False(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_FourKeysDifferentTypeEquals_ReturnsTrue()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int, string, int, string>(x => x.IntValue1,
                x => x.StrValue, x => x.IntValue2, x => x.StrValue1);

            var testObj1 = new ComparerTestObject
                {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3, StrValue1 = "World"};
            var testObj2 = new ComparerTestObject
                {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3, StrValue1 = "World"};

            Assert.True(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_FiveKeysDifferentTypeNotEquals_ReturnsFalse()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int, string, int, string, int>(
                x => x.IntValue1,
                x => x.StrValue, x => x.IntValue2, x => x.StrValue1, x => x.IntValue3);

            var testObj1 = new ComparerTestObject
                {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3, StrValue1 = "World", IntValue3 = 123};
            var testObj2 = new ComparerTestObject
                {IntValue1 = 2, StrValue = "Hello", IntValue2 = 3, StrValue1 = "World", IntValue3 = 123};

            Assert.False(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void Equals_FiveKeysDifferentTypeEquals_ReturnsTrue()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int, string, int, string, int>(
                x => x.IntValue1,
                x => x.StrValue, x => x.IntValue2, x => x.StrValue1, x => x.IntValue3);

            var testObj1 = new ComparerTestObject
                {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3, StrValue1 = "World", IntValue3 = 123};
            var testObj2 = new ComparerTestObject
                {IntValue1 = 1, StrValue = "Hello", IntValue2 = 3, StrValue1 = "World", IntValue3 = 123};

            Assert.True(comparer.Equals(testObj1, testObj2));
        }

        [Fact]
        public void GetHashCode_SingleKeyIntValue_ReturnsIntHashCode()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int>(x => x.IntValue1);

            var testObj = new ComparerTestObject {IntValue1 = 1};

            Assert.Equal(testObj.IntValue1.GetHashCode(), comparer.GetHashCode(testObj));
        }

        [Fact]
        public void GetHashCode_SingleKeyStrValue_ReturnsStrHashCode()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, string>(x => x.StrValue);

            var testObj = new ComparerTestObject {StrValue = "HelloWorld"};

            Assert.Equal(testObj.StrValue.GetHashCode(), comparer.GetHashCode(testObj));
        }

        [Fact]
        public void GetHashCode_TwoKeysIntValue_ReturnsCorrectHashCode()
        {
            var comparer =
                new MultiFuncEqualityComparer<ComparerTestObject, int, int>(x => x.IntValue1, x => x.IntValue2);

            var testObj = new ComparerTestObject {IntValue1 = 1, IntValue2 = 2};

            Assert.Equal(ArrayHashCode(testObj.IntValue1, testObj.IntValue2), comparer.GetHashCode(testObj));
        }

        [Fact]
        public void GetHashCode_ThreeKeysIntValue_ReturnsCorrectHashCode()
        {
            var comparer =
                new MultiFuncEqualityComparer<ComparerTestObject, int, int, int>(x => x.IntValue1, x => x.IntValue2,
                    x => x.IntValue3);

            var testObj = new ComparerTestObject {IntValue1 = 1, IntValue2 = 2, IntValue3 = 3};

            Assert.Equal(ArrayHashCode(testObj.IntValue1, testObj.IntValue2, testObj.IntValue3),
                comparer.GetHashCode(testObj));
        }

        [Fact]
        public void GetHashCode_FourKeysIntValue_ReturnsCorrectHashCode()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int, int, int, int>(x => x.IntValue1,
                x => x.IntValue2, x => x.IntValue3, x => x.IntValue4);

            var testObj = new ComparerTestObject {IntValue1 = 1, IntValue2 = 2, IntValue3 = 3, IntValue4 = 4};

            Assert.Equal(
                ArrayHashCode(testObj.IntValue1, testObj.IntValue2, testObj.IntValue3, testObj.IntValue4),
                comparer.GetHashCode(testObj));
        }

        [Fact]
        public void GetHashCode_FiveKeysIntValue_ReturnsCorrectHashCode()
        {
            var comparer = new MultiFuncEqualityComparer<ComparerTestObject, int, int, int, int, int>(x => x.IntValue1,
                x => x.IntValue2, x => x.IntValue3, x => x.IntValue4, x => x.IntValue5);

            var testObj = new ComparerTestObject
                {IntValue1 = 1, IntValue2 = 2, IntValue3 = 3, IntValue4 = 4, IntValue5 = 5};

            Assert.Equal(
                ArrayHashCode(testObj.IntValue1, testObj.IntValue2, testObj.IntValue3, testObj.IntValue4,
                    testObj.IntValue5),
                comparer.GetHashCode(testObj));
        }

        private static int ArrayHashCode(params object[] values)
        {
            return string.Concat(values.Select(x => x.GetHashCode())).GetHashCode();
        }
    }
}