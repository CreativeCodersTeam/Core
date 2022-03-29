using System.Collections.Generic;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Mapping;

public class DataToXmlRpcValueConverterTests
{
    [Fact]
    public void Convert_TestData_IsMappedCorrectToXmlRpcStruct()
    {
        const int intData = 1234;
        const string stringData = "TestData";

        var testData = new StructTestData
        {
            IntValue = intData,
            StringValue = stringData
        };

        var converter = new DataToXmlRpcValueConverter();

        var convertedValue = converter.Convert(testData);

        Assert.IsType<StructValue>(convertedValue);

        var xmlStructValue = (StructValue) convertedValue;

        Assert.Equal(2, xmlStructValue.Value.Count);

        var intValue = xmlStructValue.Value["IntTest"] as IntegerValue;
        var stringValue = xmlStructValue.Value["StringTest"] as StringValue;
        Assert.NotNull(intValue);
        Assert.NotNull(stringValue);

        Assert.Equal(intData, intValue.Value);
        Assert.Equal(stringData, stringValue.Value);
    }

    [Fact]
    public void Convert_StringToObject_ObjectIsString()
    {
        var converter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = new StructValue(new Dictionary<string, XmlRpcValue>
        {
            {"Value", new StringValue("Test")}
        });

        var dataObject =
            converter.Convert(xmlRpcValue, typeof(StructTestDataWithObjectValue)) as StructTestDataWithObjectValue;

        Assert.Equal("Test", dataObject?.Value);
    }
}