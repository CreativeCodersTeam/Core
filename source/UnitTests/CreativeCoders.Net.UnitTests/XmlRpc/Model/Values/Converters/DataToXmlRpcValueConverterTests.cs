using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Net.UnitTests.XmlRpc.Mapping;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values.Converters;

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

        var xmlStructValue = (StructValue)convertedValue;

        Assert.Equal(2, xmlStructValue.Value.Count);

        var intValue = xmlStructValue.Value["IntTest"] as IntegerValue;
        var stringValue = xmlStructValue.Value["StringTest"] as StringValue;
        Assert.NotNull(intValue);
        Assert.NotNull(stringValue);

        Assert.Equal(intData, intValue.Value);
        Assert.Equal(stringData, stringValue.Value);
    }

    [Fact]
    public void Convert_Enumerable_ReturnsXmlRpcArray()
    {
        var converter = new DataToXmlRpcValueConverter();

        var data = new List<object>
        {
            "Test",
            "qwertz",
            1234,
            true,
            DateTime.Now
        };

        var xmlRpcValue = converter.Convert(data) as ArrayValue;

        Assert.NotNull(xmlRpcValue);
            
        Assert.Equal(data.Count, xmlRpcValue.Value.Count());
            
        var convertedEnumerable = converter.Convert(xmlRpcValue.Value);
            
        Assert.NotNull(convertedEnumerable);
    }

    [Fact]
    public void Convert_Dictionary_ReturnsXmlRpcStruct()
    {
        var converter = new DataToXmlRpcValueConverter();

        var data = new Dictionary<string, string>
        {
            {"Prop1", "Test1a"},
            {"Prop2", "Test2b"}
            //{"Prop3", },
            //{"Prop4", },
            //{"Prop5", },
        };

        var xmlRpcValue = converter.Convert(data) as StructValue;

        Assert.NotNull(xmlRpcValue);
            
        var convertedEnumerable = converter.Convert(xmlRpcValue.Value);
            
        Assert.NotNull(convertedEnumerable);
    }

    [Fact]
    public void Convert_DictionaryWithDifferentTypes_ReturnsXmlRpcStruct()
    {
        var converter = new DataToXmlRpcValueConverter();

        var data = new Dictionary<string, object>
        {
            {"Prop1", "Test1a"},
            {"Prop2", "Test2b"},
            {"Prop3", 1234},
            {"Prop4", true},
            {"Prop5", DateTime.Now}
        };

        var xmlRpcValue = converter.Convert(data) as StructValue;
            
        Assert.NotNull(xmlRpcValue);

        var convertedEnumerable = converter.Convert(xmlRpcValue.Value);
            
        Assert.NotNull(convertedEnumerable);
    }
}