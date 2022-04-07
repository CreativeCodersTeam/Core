using System;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values;

public class DateTimeValueReaderTests
{
    [Fact]
    public void HandlesDataType_PassMatchingDataType_ReturnsTrue()
    {
        var reader = new DateTimeValueReader();

        Assert.True(reader.HandlesDataType("dateTime.iso8601"));
    }

    [Fact]
    public void HandlesDataType_PassNotMatchingDataType_ReturnsFalse()
    {
        var reader = new DateTimeValueReader();

        Assert.False(reader.HandlesDataType("string"));
    }

    [Fact]
    public void ReadValue_ForNotMatchingValue_ThrowsException()
    {
        var reader = new DateTimeValueReader();

        var xmlElement = new XElement("string");

        Assert.Throws<ParserException>(() => reader.ReadValue(xmlElement));
    }

    [Fact]
    public void ReadValue_FromElementWithDoubleValue_ReturnsCorrectDoubleValue()
    {
        var reader = new DateTimeValueReader();

        var xmlElement = new XElement("dateTime.iso8601", "20190625T11:22:33");

        var value = reader.ReadValue(xmlElement);

        Assert.IsType<DateTimeValue>(value);
        Assert.Equal(new DateTime(2019, 6, 25, 11, 22, 33), value.GetValue<DateTime>(),
            TimeSpan.FromMilliseconds(1));
    }
}
