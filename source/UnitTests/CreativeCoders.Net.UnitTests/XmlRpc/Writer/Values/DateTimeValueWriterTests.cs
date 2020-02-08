using System;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values
{
    public class DateTimeValueWriterTests
    {
        [Fact]
        public void HandlesType_MatchingType_ReturnsTrue()
        {
            var writer = new DateTimeValueWriter();

            Assert.True(writer.HandlesType(typeof(DateTimeValue)));
        }

        [Fact]
        public void HandlesType_NotMatchingType_ReturnsFalse()
        {
            var writer = new DateTimeValueWriter();

            Assert.False(writer.HandlesType(typeof(StringValue)));
        }

        [Fact]
        public void WriteTo_PassMatchingValue_ValueElementIsWrittenToParamElement()
        {
            var writer = new DateTimeValueWriter();
            var now = DateTime.Now;
            var value = new DateTimeValue(now);
            var xmlElement = new XElement("param");

            writer.WriteTo(xmlElement, value);

            var valueElement = xmlElement.XPathSelectElement("value/dateTime.iso8601");

            Assert.Equal(now.ToString("yyyyMMddTHH:mm:ss"), valueElement.Value);
        }
    }
}