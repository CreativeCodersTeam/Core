using System;
using System.Text;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values;

public class Base64ValueReaderTests
{
    [Fact]
    public void HandlesDataType_PassMatchingDataType_ReturnsTrue()
    {
        var reader = new Base64ValueReader(Encoding.UTF8);

        Assert.True(reader.HandlesDataType("base64"));
    }

    [Fact]
    public void HandlesDataType_PassNotMatchingDataType_ReturnsFalse()
    {
        var reader = new Base64ValueReader(Encoding.UTF8);

        Assert.False(reader.HandlesDataType("string"));
    }

    [Fact]
    public void ReadValue_ForNotMatchingValue_ThrowsException()
    {
        var reader = new Base64ValueReader(Encoding.UTF8);

        var xmlElement = new XElement("string");

        Assert.Throws<ParserException>(() => reader.ReadValue(xmlElement));
    }

    [Fact]
    public void ReadValue_FromElementWithDoubleValue_ReturnsCorrectDoubleValue()
    {
        var data = Encoding.UTF8.GetBytes("Test1234");
        var base64Text = Convert.ToBase64String(data);
        var reader = new Base64ValueReader(Encoding.UTF8);

        var xmlElement = new XElement("base64", base64Text);

        var value = reader.ReadValue(xmlElement);

        Assert.IsType<Base64Value>(value);
        Assert.Equal(data, value.GetValue<byte[]>());
    }
}