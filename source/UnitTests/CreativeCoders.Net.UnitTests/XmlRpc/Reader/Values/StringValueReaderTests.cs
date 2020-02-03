using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values
{
    public class StringValueReaderTests
    {
        [Fact]
        public void HandlesDataType_PassMatchingDataType_ReturnsTrue()
        {
            var reader = new StringValueReader();

            Assert.True(reader.HandlesDataType("string"));
        }

        [Fact]
        public void HandlesDataType_PassNotMatchingDataType_ReturnsFalse()
        {
            var reader = new StringValueReader();

            Assert.False(reader.HandlesDataType("int"));
        }

        [Fact]
        public void ReadValue_ForNotMatchingValue_ThrowsException()
        {
            var reader = new StringValueReader();

            var xmlElement = new XElement("int");

            Assert.Throws<ParserException>(() => reader.ReadValue(xmlElement));
        }

        [Fact]
        public void ReadValue_FromElementWithStringValue_ReturnsCorrectStringValue()
        {
            var reader = new StringValueReader();

            var xmlElement = new XElement("string", "Test1234");

            var value = reader.ReadValue(xmlElement);

            Assert.IsType<StringValue>(value);
            Assert.Equal("Test1234", value.GetValue<string>());
        }
    }
}