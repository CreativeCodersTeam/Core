using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values;

public class IntegerValueWriterTests
{
    [Fact]
    public void HandlesType_MatchingType_ReturnsTrue()
    {
        var writer = new IntegerValueWriter();

        Assert.True(writer.HandlesType(typeof(IntegerValue)));
    }

    [Fact]
    public void HandlesType_NotMatchingType_ReturnsFalse()
    {
        var writer = new IntegerValueWriter();

        Assert.False(writer.HandlesType(typeof(StringValue)));
    }

    [Fact]
    public void WriteTo_PassMatchingValue_ValueElementIsWrittenToParamElement()
    {
        var writer = new IntegerValueWriter();

        var value = new IntegerValue(1234);
        var xmlElement = new XElement("param");

        writer.WriteTo(xmlElement, value);

        var valueElement = xmlElement.XPathSelectElement("value/i4");

        Assert.NotNull(valueElement);
        Assert.Equal("1234", valueElement.Value);
    }
}