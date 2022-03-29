using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values;

public class ArrayValueWriterTests
{
    [Fact]
    public void HandlesType_MatchingType_ReturnsTrue()
    {
        var writers = A.Fake<IValueWriters>();
        var writer = new ArrayValueWriter(writers);

        Assert.True(writer.HandlesType(typeof(ArrayValue)));
    }

    [Fact]
    public void HandlesType_NotMatchingType_ReturnsFalse()
    {
        var writers = A.Fake<IValueWriters>();
        var writer = new ArrayValueWriter(writers);

        Assert.False(writer.HandlesType(typeof(StringValue)));
    }

    [Fact]
    public void WriteTo_PassMatchingValue_ValueElementIsWrittenToParamElement()
    {
        var integerValue = new IntegerValue(2345);
        var xmlElement = new XElement("param");

        var writerAction = new Action<XElement, XmlRpcValue>((xml, xmlRpcValue) => xml.Add(xmlRpcValue.Data.ToString()));
        var integerWriter = A.Fake<IValueWriter>();
        A.CallTo(() => integerWriter.WriteTo(A<XElement>.Ignored, integerValue)).Invokes(writerAction);

        var writers = A.Fake<IValueWriters>();
        A.CallTo(() => writers.GetWriter(typeof(IntegerValue)))
            .Returns(integerWriter);

        var writer = new ArrayValueWriter(writers);
            
        var value = new ArrayValue(new []{integerValue});

        writer.WriteTo(xmlElement, value);

        var valueElements = xmlElement.XPathSelectElements("value/array/data").ToArray();

        Assert.Single(valueElements);
        Assert.Equal(integerValue.Value.ToString(), valueElements.First().Value);
    }
}