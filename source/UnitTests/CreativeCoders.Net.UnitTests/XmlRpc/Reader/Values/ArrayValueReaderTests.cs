using System.Linq;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values;

public class ArrayValueReaderTests
{
    [Fact]
    public void HandlesDataType_PassMatchingDataType_ReturnsTrue()
    {
        var valueReaders = A.Fake<IValueReaders>();
        var reader = new ArrayValueReader(valueReaders);

        Assert.True(reader.HandlesDataType("array"));
    }

    [Fact]
    public void HandlesDataType_PassNotMatchingDataType_ReturnsFalse()
    {
        var valueReaders = A.Fake<IValueReaders>();
        var reader = new ArrayValueReader(valueReaders);

        Assert.False(reader.HandlesDataType("string"));
    }

    [Fact]
    public void ReadValue_ForNotMatchingValue_ThrowsException()
    {
        var valueReaders = A.Fake<IValueReaders>();
        var reader = new ArrayValueReader(valueReaders);

        var xmlElement = new XElement("string");

        Assert.Throws<ParserException>(() => reader.ReadValue(xmlElement));
    }

    [Fact]
    public void ReadValue_FromElementWithArrayValue_ReturnsCorrectArrayValue()
    {
        var integerValueReader = A.Fake<IValueReader>();
        A.CallTo(() => integerValueReader.ReadValue(A<XElement>.Ignored)).Returns(new IntegerValue(2345));
        var valueReaders = A.Fake<IValueReaders>();
        A.CallTo(() => valueReaders.ReadValue(A<XElement>.Ignored)).Returns(new IntegerValue(2345));

        var reader = new ArrayValueReader(valueReaders);

        var xmlElement = new XElement("array", new XElement("data", new XElement("value", new XElement("int"))));

        var value = reader.ReadValue(xmlElement);

        Assert.IsType<ArrayValue>(value);
        var arrayValue = (ArrayValue) value;
        Assert.Single(arrayValue.Value);
        var firstValue = arrayValue.Value.First();
        Assert.IsType<IntegerValue>(firstValue);
        Assert.Equal(2345, firstValue.GetValue<int>());
    }
}