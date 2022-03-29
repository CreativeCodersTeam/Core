using System.Linq;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model;

public class XmlRpcMethodResultTests
{
    [Fact]
    public void Ctor_NoParameter_EmptyNotFaultedResult()
    {
        var methodResult = new XmlRpcMethodResult();

        Assert.Empty(methodResult.Values);
        Assert.False(methodResult.IsFaulted);
        Assert.Null(methodResult.FaultString);
        Assert.Equal(0, methodResult.FaultCode);
    }

    [Fact]
    public void Ctor_FaultParameters_FaultedResultWithStringAndCodeSet()
    {
        var methodResult = new XmlRpcMethodResult(1234, "This is a error");

        Assert.Empty(methodResult.Values);
        Assert.True(methodResult.IsFaulted);
        Assert.Equal("This is a error", methodResult.FaultString);
        Assert.Equal(1234, methodResult.FaultCode);
    }

    [Fact]
    public void Ctor_ValueParameters_ValuesAreSet()
    {
        var stringValue = new StringValue("SomeValue");
        var intValue = new IntegerValue(12345);

        var methodResult = new XmlRpcMethodResult(stringValue, intValue);

        Assert.False(methodResult.IsFaulted);
        Assert.Null(methodResult.FaultString);
        Assert.Equal(0, methodResult.FaultCode);

        Assert.Equal(2, methodResult.Values.Count());

        Assert.Same(stringValue, methodResult.Values.First());
        Assert.Same(intValue, methodResult.Values.Last());

        Assert.Equal("SomeValue", methodResult.Values.First().GetValue<string>());
        Assert.Equal(12345, methodResult.Values.Last().GetValue<int>());
    }
}