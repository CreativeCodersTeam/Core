using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values;

public class IntegerValueTests
{
    [Fact]
    public void Ctor_DateTimeNowValue_ValueIsDateTimeNow()
    {
        var value = new IntegerValue(1234);

        Assert.Equal(1234, value.Value);
    }
}
