using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values;

public class IntegerValueReaderTests
{
    [Fact]
    public void HandlesDataType_PassMatchingDataType_ReturnsTrue()
    {
        var reader = new IntegerValueReader();

        Assert.True(reader.HandlesDataType("int"));
        Assert.True(reader.HandlesDataType("i4"));
    }

    [Fact]
    public void HandlesDataType_PassNotMatchingDataType_ReturnsFalse()
    {
        var reader = new IntegerValueReader();

        Assert.False(reader.HandlesDataType("string"));
    }

    [Fact]
    public void ReadValue_ForNotMatchingValue_ThrowsException()
    {
        var reader = new IntegerValueReader();

        var xmlElement = new XElement("string");

        Assert.Throws<ParserException>(() => reader.ReadValue(xmlElement));
    }

    [Fact]
    public void ReadValue_FromElementWithIntegerValue_ReturnsCorrectIntegerValue()
    {
        var reader = new IntegerValueReader();

        var xmlElement = new XElement("int", "1234");

        var value = reader.ReadValue(xmlElement);

        Assert.IsType<IntegerValue>(value);
        Assert.Equal(1234, value.GetValue<int>());
    }
}