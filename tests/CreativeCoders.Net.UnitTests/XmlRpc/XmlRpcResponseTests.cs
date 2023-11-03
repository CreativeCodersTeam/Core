using System;
using System.Linq;
using CreativeCoders.Net.XmlRpc;
using CreativeCoders.Net.XmlRpc.Model;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc;

public class XmlRpcResponseTests
{
    [Fact]
    public void Ctor_WithNullResults_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new XmlRpcResponse(null, false));
    }

    [Fact]
    public void Ctor_WithMethod_HasMethodSetToParameter()
    {
        var methodResult = new XmlRpcMethodResult();

        var request = new XmlRpcResponse(new[] {methodResult}, false);

        Assert.Single(request.Results);
        Assert.Same(methodResult, request.Results.First());
    }

    [Fact]
    public void Ctor_WithMethods_HasMethodsSetToParameter()
    {
        var methodResult = new XmlRpcMethodResult();
        var methodResult1 = new XmlRpcMethodResult();

        var request = new XmlRpcResponse(new[] {methodResult, methodResult1}, false);

        Assert.Equal(2, request.Results.Count());
        Assert.Equal(request.Results, new[] {methodResult, methodResult1});
    }

    [Fact]
    public void Ctor_WithMultiCallFalse_MultiCallPropertyIsFalse()
    {
        var methodResult = new XmlRpcMethodResult();

        var request = new XmlRpcResponse(new[] {methodResult}, false);

        Assert.False(request.IsMultiCall);
    }

    [Fact]
    public void Ctor_WithMultiCallTrue_MultiCallPropertyIsTrue()
    {
        var methodResult = new XmlRpcMethodResult();

        var request = new XmlRpcResponse(new[] {methodResult}, true);

        Assert.True(request.IsMultiCall);
    }
}
