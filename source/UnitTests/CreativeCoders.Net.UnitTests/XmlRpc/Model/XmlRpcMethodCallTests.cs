using System;
using System.Linq;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model;

public class XmlRpcMethodCallTests
{
    [Fact]
    public void Ctor_NameNullOrWhiteSpace_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new XmlRpcMethodCall(null));
        Assert.Throws<ArgumentException>(() => new XmlRpcMethodCall(string.Empty));
        Assert.Throws<ArgumentException>(() => new XmlRpcMethodCall(" "));
    }

    [Fact]
    public void Ctor_NameIsCorrect_NameIsSet()
    {
        var methodCall = new XmlRpcMethodCall("Test");

        Assert.Equal("Test", methodCall.Name);
    }

    [Fact]
    public void Ctor_ParametersAreGiven_ParametersAreSet()
    {
        var stringValue = new StringValue("SomeValue");
        var intValue = new IntegerValue(1234);

        var methodCall = new XmlRpcMethodCall("Test", stringValue, intValue);

        Assert.Equal(2, methodCall.Parameters.Count());

        Assert.Same(stringValue, methodCall.Parameters.First());
        Assert.Same(intValue, methodCall.Parameters.Last());

        Assert.Equal("SomeValue", methodCall.Parameters.First().GetValue<string>());
        Assert.Equal(1234, methodCall.Parameters.Last().GetValue<int>());
    }

    [Fact]
    public void Ctor_AddParameter_ParameterIsAdd()
    {
        var stringValue = new StringValue("SomeValue");
        var intValue = new IntegerValue(1234);
        var addedValue = new StringValue("This is added");

        var methodCall = new XmlRpcMethodCall("Test", stringValue, intValue);

        methodCall.AddParameter(addedValue);

        Assert.Equal(3, methodCall.Parameters.Count());

        Assert.Same(stringValue, methodCall.Parameters.First());
        Assert.Same(intValue, methodCall.Parameters.Skip(1).First());
        Assert.Same(addedValue, methodCall.Parameters.Last());

        Assert.Equal("SomeValue", methodCall.Parameters.First().GetValue<string>());
        Assert.Equal(1234, methodCall.Parameters.Skip(1).First().GetValue<int>());
        Assert.Equal("This is added", methodCall.Parameters.Last().GetValue<string>());
    }
}