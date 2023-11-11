using CreativeCoders.Core.Comparing;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Comparing;

public class DelegateEqualityComparerTests
{
    [Fact]
    public void Equals_CompareEqualTwoIntegers_ReturnTrue()
    {
        var comparer = new DelegateEqualityComparer<int>((value1, value2) => value1 == value2);

        Assert.True(comparer.Equals(1234, 1234));
    }

    [Fact]
    public void Equals_CompareNotEqualTwoIntegers_ReturnFalse()
    {
        var comparer = new DelegateEqualityComparer<int>((value1, value2) => value1 == value2);

        Assert.False(comparer.Equals(1234, 2345));
    }

    [Fact]
    public void GetHashCode_()
    {
        var comparer =
            new DelegateEqualityComparer<int>((value1, value2) => value1 == value2, value => value);

        Assert.Equal(1234, comparer.GetHashCode(1234));
    }
}
