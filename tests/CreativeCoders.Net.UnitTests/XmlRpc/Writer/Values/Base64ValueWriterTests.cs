using System;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values;

public class Base64ValueWriterTests
{
    [Fact]
    public void HandlesType_MatchingType_ReturnsTrue()
    {
        var writer = new Base64ValueWriter();

        Assert.True(writer.HandlesType(typeof(Base64Value)));
    }

    [Fact]
    public void HandlesType_NotMatchingType_ReturnsFalse()
    {
        var writer = new Base64ValueWriter();

        Assert.False(writer.HandlesType(typeof(StringValue)));
    }

    [Fact]
    public void WriteTo_PassMatchingValue_ValueElementIsWrittenToParamElement()
    {
        var writer = new Base64ValueWriter();
        var data = Encoding.UTF8.GetBytes("Test1234");
        var value = new Base64Value(data, Encoding.UTF8);
        var xmlElement = new XElement("param");

        writer.WriteTo(xmlElement, value);

        var valueElement = xmlElement.XPathSelectElement("value/base64");

        Assert.NotNull(valueElement);
        Assert.Equal(Convert.ToBase64String(data), valueElement.Value);
    }
}
