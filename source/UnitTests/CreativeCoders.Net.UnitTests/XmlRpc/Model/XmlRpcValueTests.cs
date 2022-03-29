using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model;

public class XmlRpcValueTests
{
    [Fact]
    public void GetValue_WrongGenericParameter_ReturnGenericDefault()
    {
        var intValue = new IntegerValue(2345);

        var value = intValue.GetValue<string>();

        Assert.Null(value);
    }
}