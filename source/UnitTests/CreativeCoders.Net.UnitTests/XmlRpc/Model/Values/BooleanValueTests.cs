using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values;

public class BooleanValueTests
{
    [Fact]
    public void Ctor_TrueValue_ValueIsTrue()
    {
        var value = new BooleanValue(true);

        Assert.True(value.Value);
    }

    [Fact]
    public void Ctor_FalseValue_ValueIsFalse()
    {
        var value = new BooleanValue(false);

        Assert.False(value.Value);
    }
}