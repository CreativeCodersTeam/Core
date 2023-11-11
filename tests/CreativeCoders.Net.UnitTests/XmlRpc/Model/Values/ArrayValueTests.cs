using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values;

public class ArrayValueTests
{
    [Fact]
    public void Ctor_NullParameter_ValueIsEmpty()
    {
        var value = new ArrayValue(null);
        var xmlRpcValue = (XmlRpcValue) value;

        Assert.Empty(value.Value);
        Assert.Empty(xmlRpcValue.GetValue<IEnumerable<XmlRpcValue>>());
    }

    [Fact]
    public void Ctor_EmptyCollection_ValueIsEmpty()
    {
        var value = new ArrayValue(Array.Empty<XmlRpcValue>());
        var xmlRpcValue = (XmlRpcValue) value;

        Assert.Empty(value.Value);
        Assert.Empty(xmlRpcValue.GetValue<IEnumerable<XmlRpcValue>>());
    }

    [Fact]
    public void Ctor_CollectionWithValues_ValueCollectionIsEqual()
    {
        var stringValue = new StringValue("SomeValue");
        var intValue = new IntegerValue(1234);

        var value = new ArrayValue(new XmlRpcValue[] {stringValue, intValue});
        var xmlRpcValue = (XmlRpcValue) value;

        Assert.Equal(2, value.Value.Count());
        Assert.Equal(2, xmlRpcValue.GetValue<IEnumerable<XmlRpcValue>>().Count());
        Assert.Same(stringValue, value.Value.First());
        Assert.Same(intValue, value.Value.Last());
    }
}
