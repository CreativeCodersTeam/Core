using System.Linq;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values
{
    public class StructValueReaderTests
    {
        [Fact]
        public void HandlesDataType_PassMatchingDataType_ReturnsTrue()
        {
            var valueReaders = A.Fake<IValueReaders>();
            var reader = new StructValueReader(valueReaders);

            Assert.True(reader.HandlesDataType("struct"));
        }

        [Fact]
        public void HandlesDataType_PassNotMatchingDataType_ReturnsFalse()
        {
            var valueReaders = A.Fake<IValueReaders>();
            var reader = new StructValueReader(valueReaders);

            Assert.False(reader.HandlesDataType("string"));
        }

        [Fact]
        public void ReadValue_ForNotMatchingValue_ThrowsException()
        {
            var valueReaders = A.Fake<IValueReaders>();
            var reader = new StructValueReader(valueReaders);

            var xmlElement = new XElement("string");

            Assert.Throws<ParserException>(() => reader.ReadValue(xmlElement));
        }

        [Fact]
        public void ReadValue_FromElementWithArrayValue_ReturnsCorrectArrayValue()
        {
            var valueReaders = A.Fake<IValueReaders>();
            A.CallTo(() => valueReaders.ReadValue(A<XElement>.Ignored)).Returns(new StringValue("2345"));

            var reader = new StructValueReader(valueReaders);

            var xmlElement = new XElement("struct", new XElement("member", new XElement("name", "TextProp"), new XElement("value", new XElement("string", "Value1234"))));

            var value = reader.ReadValue(xmlElement);

            Assert.IsType<StructValue>(value);
            var arrayValue = (StructValue)value;
            Assert.Single(arrayValue.Value);
            var (key, xmlRpcValue) = arrayValue.Value.First();
            Assert.Equal("TextProp", key);
            Assert.Equal("2345", xmlRpcValue.GetValue<string>());
            //Assert.IsType<IntegerValue>(firstValue);
            //Assert.Equal(2345, firstValue.GetValue<int>());
        }
    }
}