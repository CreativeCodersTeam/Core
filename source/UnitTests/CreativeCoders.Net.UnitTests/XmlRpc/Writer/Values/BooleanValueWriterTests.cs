using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values
{
    public class BooleanValueWriterTests
    {
        [Fact]
        public void HandlesType_MatchingType_ReturnsTrue()
        {
            var writer = new BooleanValueWriter();

            Assert.True(writer.HandlesType(typeof(BooleanValue)));
        }

        [Fact]
        public void HandlesType_NotMatchingType_ReturnsFalse()
        {
            var writer = new BooleanValueWriter();

            Assert.False(writer.HandlesType(typeof(StringValue)));
        }

        [Fact]
        public void WriteTo_PassTrueValue_ValueElementOneIsWrittenToParamElement()
        {
            var writer = new BooleanValueWriter();

            var value = new BooleanValue(true);
            var xmlElement = new XElement("param");

            writer.WriteTo(xmlElement, value);

            var valueElement = xmlElement.XPathSelectElement("value/boolean");

            Assert.NotNull(valueElement);
            Assert.Equal("1", valueElement.Value);
        }

        [Fact]
        public void WriteTo_PassFalseValue_ValueElementZeroIsWrittenToParamElement()
        {
            var writer = new BooleanValueWriter();

            var value = new BooleanValue(false);
            var xmlElement = new XElement("param");

            writer.WriteTo(xmlElement, value);

            var valueElement = xmlElement.XPathSelectElement("value/boolean");

            Assert.NotNull(valueElement);
            Assert.Equal("0", valueElement.Value);
        }
    }
}