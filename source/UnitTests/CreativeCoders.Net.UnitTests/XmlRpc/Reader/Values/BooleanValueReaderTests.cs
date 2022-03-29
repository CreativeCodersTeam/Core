using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values;

public class BooleanValueReaderTests
{
    [Fact]
    public void HandlesDataType_PassMatchingDataType_ReturnsTrue()
    {
        var reader = new BooleanValueReader();

        Assert.True(reader.HandlesDataType("boolean"));
    }

    [Fact]
    public void HandlesDataType_PassNotMatchingDataType_ReturnsFalse()
    {
        var reader = new BooleanValueReader();

        Assert.False(reader.HandlesDataType("string"));
    }

    [Fact]
    public void ReadValue_ForNotMatchingValue_ThrowsException()
    {
        var reader = new BooleanValueReader();

        var xmlElement = new XElement("string");

        Assert.Throws<ParserException>(() => reader.ReadValue(xmlElement));
    }

    [Fact]
    public void ReadValue_XmlElementIsOne_ReturnsBooleanValueWithValueSetToTrue()
    {
        var reader = new BooleanValueReader();

        var xmlElement = new XElement("boolean", "1");

        var value = reader.ReadValue(xmlElement);

        Assert.IsType<BooleanValue>(value);
        Assert.True(value.GetValue<bool>());
    }

    [Fact]
    public void ReadValue_XmlElementIsZero_ReturnsBooleanValueWithValueSetToFalse()
    {
        var reader = new BooleanValueReader();

        var xmlElement = new XElement("boolean", "0");

        var value = reader.ReadValue(xmlElement);

        Assert.IsType<BooleanValue>(value);
        Assert.False(value.GetValue<bool>());
    }
}