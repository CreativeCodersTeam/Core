using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Writer;
using CreativeCoders.Net.XmlRpc.Writer.Values;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Writer.Values
{
    public class StructValueWriterTests
    {
        [Fact]
        public void HandlesType_MatchingType_ReturnsTrue()
        {
            var writers = A.Fake<IValueWriters>();
            var writer = new StructValueWriter(writers);

            Assert.True(writer.HandlesType(typeof(StructValue)));
        }

        [Fact]
        public void HandlesType_NotMatchingType_ReturnsFalse()
        {
            var writers = A.Fake<IValueWriters>();
            var writer = new StructValueWriter(writers);

            Assert.False(writer.HandlesType(typeof(StringValue)));
        }

        [Fact]
        public void WriteTo_PassMatchingValue_ValueElementIsWrittenToParamElement()
        {
            var integerValue = new IntegerValue(2345);
            var xmlElement = new XElement("param");

            var writerAction = new Action<XElement, XmlRpcValue>((xml, xmlRpcValue) => xml.Add(new XElement("value", xmlRpcValue.Data.ToString())));
            var integerWriter = A.Fake<IValueWriter>();
            A.CallTo(() => integerWriter.WriteTo(A<XElement>.Ignored, integerValue)).Invokes(writerAction);

            var writers = A.Fake<IValueWriters>();
            A.CallTo(() => writers.GetWriter(typeof(IntegerValue)))
                .Returns(integerWriter);

            var writer = new StructValueWriter(writers);

            var value = new StructValue(new Dictionary<string, XmlRpcValue>{{"TestProp", integerValue}});

            writer.WriteTo(xmlElement, value);

            var memberElements = xmlElement.XPathSelectElements("value/struct/member").ToArray();

            Assert.Single(memberElements);
            var memberElement = memberElements.First();
            var subElements = memberElement.Elements().ToArray();
            Assert.Equal(2, subElements.Length);
            var nameElement = subElements.First();
            Assert.Equal("TestProp", nameElement.Value);
            Assert.Equal(integerValue.Value.ToString(), subElements.Last().Value);
        }
    }
}