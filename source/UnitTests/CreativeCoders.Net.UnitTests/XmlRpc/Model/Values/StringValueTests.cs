using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values;

public class StringValueTests
{
    [Fact]
    public void Ctor_DateTimeNowValue_ValueIsDateTimeNow()
    {
        var value = new StringValue("1234");

        Assert.Equal("1234", value.Value);
    }
}
