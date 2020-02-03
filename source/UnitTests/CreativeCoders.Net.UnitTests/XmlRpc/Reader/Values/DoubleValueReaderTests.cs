using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Exceptions;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader.Values
{
    public class DoubleValueReaderTests
    {
        [Fact]
        public void HandlesDataType_PassMatchingDataType_ReturnsTrue()
        {
            var reader = new DoubleValueReader();

            Assert.True(reader.HandlesDataType("double"));
        }

        [Fact]
        public void HandlesDataType_PassNotMatchingDataType_ReturnsFalse()
        {
            var reader = new DoubleValueReader();

            Assert.False(reader.HandlesDataType("string"));
        }

        [Fact]
        public void ReadValue_ForNotMatchingValue_ThrowsException()
        {
            var reader = new DoubleValueReader();

            var xmlElement = new XElement("string");

            Assert.Throws<ParserException>(() => reader.ReadValue(xmlElement));
        }

        [Fact]
        public void ReadValue_FromElementWithDoubleValue_ReturnsCorrectDoubleValue()
        {
            var reader = new DoubleValueReader();

            var xmlElement = new XElement("double", "1234.56");

            var value = reader.ReadValue(xmlElement);

            Assert.IsType<DoubleValue>(value);
            Assert.Equal(1234.56, value.GetValue<double>());
        }
    }
}