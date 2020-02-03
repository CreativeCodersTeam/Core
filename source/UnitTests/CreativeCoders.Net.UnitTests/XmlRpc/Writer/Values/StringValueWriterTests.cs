using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values
{
    public class StringValueWriterTests
    {
        [Fact]
        public void HandlesType_MatchingType_ReturnsTrue()
        {
            var writer = new StringValueWriter();

            Assert.True(writer.HandlesType(typeof(StringValue)));
        }

        [Fact]
        public void HandlesType_NotMatchingType_ReturnsFalse()
        {
            var writer = new StringValueWriter();

            Assert.False(writer.HandlesType(typeof(IntegerValue)));
        }

        [Fact]
        public void WriteTo_PassMatchingValue_ValueElementIsWrittenToParamElement()
        {
            var writer = new StringValueWriter();

            var value = new StringValue("Test1234");
            var xmlElement = new XElement("param");

            writer.WriteTo(xmlElement, value);

            var valueElement = xmlElement.XPathSelectElement("value/string");

            Assert.Equal("Test1234", valueElement.Value);
        }
    }
}