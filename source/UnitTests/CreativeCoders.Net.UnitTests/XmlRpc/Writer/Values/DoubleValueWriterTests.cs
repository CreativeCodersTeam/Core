using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values;

public class DoubleValueWriterTests
{
    [Fact]
    public void HandlesType_MatchingType_ReturnsTrue()
    {
        var writer = new DoubleValueWriter();

        Assert.True(writer.HandlesType(typeof(DoubleValue)));
    }

    [Fact]
    public void HandlesType_NotMatchingType_ReturnsFalse()
    {
        var writer = new DoubleValueWriter();

        Assert.False(writer.HandlesType(typeof(StringValue)));
    }

    [Fact]
    public void WriteTo_PassMatchingValue_ValueElementIsWrittenToParamElement()
    {
        var writer = new DoubleValueWriter();

        var value = new DoubleValue(1234.56);
        var xmlElement = new XElement("param");

        writer.WriteTo(xmlElement, value);

        var valueElement = xmlElement.XPathSelectElement("value/double");

        Assert.NotNull(valueElement);
        Assert.Equal("1234.56", valueElement.Value);
    }
}