using CreativeCoders.Core.Comparing;
using CreativeCoders.Core.UnitTests.Comparing.TestData;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Comparing;

public class FuncEqualityComparerTests
{
    [Fact]
    public void Equals_NotEqual_ReturnsFalse()
    {
        var comparer = new FuncEqualityComparer<ComparerTestObject, int>(x => x.IntValue1);

        var testObj1 = new ComparerTestObject {IntValue1 = 1};
        var testObj2 = new ComparerTestObject {IntValue1 = 2};

        Assert.False(comparer.Equals(testObj1, testObj2));
    }

    [Fact]
    public void Equals_Equal_ReturnsTrue()
    {
        var comparer = new FuncEqualityComparer<ComparerTestObject, int>(x => x.IntValue1);

        var testObj1 = new ComparerTestObject {IntValue1 = 1};
        var testObj2 = new ComparerTestObject {IntValue1 = 1};

        Assert.True(comparer.Equals(testObj1, testObj2));
    }

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        var comparer = new FuncEqualityComparer<ComparerTestObject, string>(x => x.StrValue);

        var testObj = new ComparerTestObject {StrValue = "HelloWorld"};

        Assert.True(comparer.Equals(testObj, testObj));
    }

    [Fact]
    public void Equals_ReferenceOtherNull_ReturnsFalse()
    {
        var comparer = new FuncEqualityComparer<ComparerTestObject, string>(x => x.StrValue);

        var testObj1 = new ComparerTestObject {StrValue = "HelloWorld"};

        Assert.False(comparer.Equals(testObj1, null));
    }

    [Fact]
    public void GetHashCode_IntValue_ReturnsIntHashCode()
    {
        var comparer = new FuncEqualityComparer<ComparerTestObject, int>(x => x.IntValue1);

        var testObj1 = new ComparerTestObject {IntValue1 = 1};

        Assert.Equal(testObj1.IntValue1.GetHashCode(), comparer.GetHashCode(testObj1));
    }

    [Fact]
    public void GetHashCode_StrValue_ReturnsIntHashCode()
    {
        var comparer = new FuncEqualityComparer<ComparerTestObject, string>(x => x.StrValue);

        var testObj1 = new ComparerTestObject {StrValue = "HelloWorld"};

        Assert.Equal(testObj1.StrValue.GetHashCode(), comparer.GetHashCode(testObj1));
    }

    [Fact]
    public void GetHashCode_StrValueNull_ReturnsZero()
    {
        var comparer = new FuncEqualityComparer<ComparerTestObject, string>(x => x.StrValue);

        var testObj1 = new ComparerTestObject {StrValue = null};

        Assert.Equal(0, comparer.GetHashCode(testObj1));
    }
}
