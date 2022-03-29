using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values.Converters;

public class XmlRpcValueToDataConverterTests
{
    [Fact]
    public void Convert_SimpleDataType_ReturnsCorrectValue()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new StringValue("qwertz");

        var value = converter.Convert(xmlRpcValue, typeof(string)) as string;

        Assert.NotNull(value);
        Assert.Equal("qwertz", value);
    }

    [Fact]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public void Convert_ArrayDataTypeAsEnumerable_ReturnsCorrectValue()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new ArrayValue(new []{new StringValue("1234"), new StringValue("abcd")});

        var value = converter.Convert(xmlRpcValue, typeof(IEnumerable<string>)) as IEnumerable<string>;
            
        Assert.NotNull(value);
        Assert.Equal(2, value.Count());
        Assert.Equal("1234", value.First());
        Assert.Equal("abcd", value.Last());
    }

    [Fact]
    public void Convert_ArrayDataTypeAsArray_ReturnsCorrectValue()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new ArrayValue(new[] { new StringValue("1234"), new StringValue("abcd") });

        var value = converter.Convert(xmlRpcValue, typeof(string[])) as string[];
            
        Assert.NotNull(value);
        Assert.Equal(2, value.Length);
        Assert.Equal("1234", value[0]);
        Assert.Equal("abcd", value[1]);
    }

    [Fact]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public void Convert_ArrayDataTypeAsEnumerableXmlRpcValue_ReturnsCorrectValue()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new ArrayValue(new[] { new StringValue("1234"), new StringValue("abcd") });

        var value = converter.Convert(xmlRpcValue, typeof(IEnumerable<XmlRpcValue>)) as IEnumerable<XmlRpcValue>;
            
        Assert.NotNull(value);
        Assert.Equal(2, value.Count());

        var firstValue = value.First() as StringValue;
        var secondValue = value.Last() as StringValue;
            
        Assert.NotNull(firstValue);
        Assert.NotNull(secondValue);
            
        Assert.Equal("1234", firstValue.Value);
        Assert.Equal("abcd", secondValue.Value);
    }

    [Fact]
    public void Convert_ArrayDataTypeAsArrayXmlRpcValue_ReturnsCorrectValue()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new ArrayValue(new[] { new StringValue("1234"), new StringValue("abcd") });

        var value = converter.Convert(xmlRpcValue, typeof(XmlRpcValue[])) as XmlRpcValue[];
            
        Assert.NotNull(value);
        Assert.Equal(2, value.Length);

        var firstValue = value[0] as StringValue;
        var secondValue = value[1] as StringValue;
            
        Assert.NotNull(firstValue);
        Assert.NotNull(secondValue);
            
        Assert.Equal("1234", firstValue.Value);
        Assert.Equal("abcd", secondValue.Value);
    }

    [Fact]
    public void Convert_DictionaryWithXmlRpcValue_ReturnsCorrectValue()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new StructValue(new Dictionary<string, XmlRpcValue>
        {
            {"TestProp1", new StringValue("1234")},
            {"TestProp2", new StringValue("abcd")}
        });

        var value = converter.Convert(xmlRpcValue, typeof(Dictionary<string, XmlRpcValue>)) as IDictionary<string, XmlRpcValue>;
            
        Assert.NotNull(value);
            
        Assert.Equal(2, value.Keys.Count);

        var firstValue = value["TestProp1"] as StringValue;
        var secondValue = value["TestProp2"] as StringValue;
            
        Assert.NotNull(firstValue);
        Assert.NotNull(secondValue);
            
        Assert.Equal("1234", firstValue.Value);
        Assert.Equal("abcd", secondValue.Value);
    }

    [Fact]
    public void Convert_DictionaryWithString_ReturnsCorrectValue()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new StructValue(new Dictionary<string, XmlRpcValue>
        {
            {"TestProp1", new StringValue("1234")},
            {"TestProp2", new StringValue("abcd")}
        });

        var value = converter.Convert(xmlRpcValue, typeof(Dictionary<string, string>)) as IDictionary<string, string>;

        Assert.NotNull(value);
            
        Assert.Equal(2, value.Keys.Count);

        var firstValue = value["TestProp1"];
        var secondValue = value["TestProp2"];
            
        Assert.Equal("1234", firstValue);
        Assert.Equal("abcd", secondValue);
    }

    [Fact]
    public void Convert_DictionaryWithDifferentObjectTypes_ReturnsCorrectValue()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new StructValue(new Dictionary<string, XmlRpcValue>
        {
            {"TestProp1", new StringValue("Hallo")},
            {"TestProp2", new StringValue("abcd")},
            {"TestProp3", new IntegerValue(5678)},
            {"TestProp4", new BooleanValue(true)}
        });

        var value = converter.Convert(xmlRpcValue, typeof(Dictionary<string, object>)) as IDictionary<string, object>;
            
        Assert.NotNull(value);
            
        Assert.Equal(4, value.Keys.Count);

        var firstStringValue = value["TestProp1"];
        var secondStringValue = value["TestProp2"];
        var integerValue = value["TestProp3"];
        var boolValue = value["TestProp4"];
            
        Assert.Equal("Hallo", firstStringValue);
        Assert.Equal("abcd", secondStringValue);
        Assert.Equal(5678, integerValue);
        Assert.Equal(true, boolValue);
    }
}