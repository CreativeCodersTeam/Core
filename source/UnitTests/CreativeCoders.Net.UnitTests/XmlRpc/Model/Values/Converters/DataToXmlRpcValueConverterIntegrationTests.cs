using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Net.UnitTests.XmlRpc.Mapping;
using CreativeCoders.Net.XmlRpc.Model;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Model.Values.Converters;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Model.Values.Converters;

public class DataToXmlRpcValueConverterIntegrationTests
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
    public void Convert_TestDataWithSubProperty_IsMappedCorrectToXmlRpcStruct()
    {
        const int intData = 1234;
        const string stringData = "TestData";

        var testData = new StructTestData
        {
            IntValue = intData,
            StringValue = stringData,
            SubData = new SubTestData
            {
                Id = 5678,
                Name = "Its"
            }
        };

        var converter = new DataToXmlRpcValueConverter();

        var convertedValue = converter.Convert(testData);

        Assert.IsType<StructValue>(convertedValue);

        var xmlStructValue = (StructValue)convertedValue;

        Assert.Equal(3, xmlStructValue.Value.Count);

        var intValue = xmlStructValue.Value["IntTest"] as IntegerValue;
        var stringValue = xmlStructValue.Value["StringTest"] as StringValue;
        var subDataValue = xmlStructValue.Value["SubData"] as StructValue;
        Assert.NotNull(intValue);
        Assert.NotNull(stringValue);
        Assert.NotNull(subDataValue);

        Assert.Equal(intData, intValue.Value);
        Assert.Equal(stringData, stringValue.Value);
        Assert.Equal(2, subDataValue.Value.Count);

        var idValue = subDataValue.Value["Id"] as IntegerValue;
        var nameValue = subDataValue.Value["Name"] as StringValue;

        Assert.NotNull(idValue);
        Assert.NotNull(nameValue);
        Assert.Equal(5678, idValue.Value);
        Assert.Equal("Its", nameValue.Value);
    }

    [Fact]
    public void Convert_TestDataWithArrayProperty_IsMappedCorrectToXmlRpcStruct()
    {
        const int intData = 1234;
        const string stringData = "TestData";

        var testData = new StructTestDataWithArray
        {
            IntValue = intData,
            StringValue = stringData,
            SubItems = new[]
            {
                new SubTestData
                {
                    Id = 2345,
                    Name = "Item1"
                },
                new SubTestData
                {
                    Id = 3456,
                    Name = "Item2"
                }
            }
        };

        var converter = new DataToXmlRpcValueConverter();

        var convertedValue = converter.Convert(testData);

        Assert.IsType<StructValue>(convertedValue);

        var xmlStructValue = (StructValue)convertedValue;

        CheckTestData(xmlStructValue, intData, stringData, 2345, "Item1", 3456, "Item2");
    }

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private static void CheckTestData(StructValue xmlStructValue, int intData, string stringData, int firstId, string firstName, int secondId, string secondName)
    {
        Assert.Equal(3, xmlStructValue.Value.Count);

        var intValue = xmlStructValue.Value["IntTest"] as IntegerValue;
        var stringValue = xmlStructValue.Value["StringTest"] as StringValue;
        var subItemsValue = xmlStructValue.Value["SubItems"] as ArrayValue;
        Assert.NotNull(intValue);
        Assert.NotNull(stringValue);
        Assert.NotNull(subItemsValue);

        Assert.Equal(2, subItemsValue.Value.Count());

        var firstSubItem = subItemsValue.Value.First() as StructValue;
        var secondSubItem = subItemsValue.Value.Last() as StructValue;

        Assert.NotNull(firstSubItem);
        Assert.NotNull(secondSubItem);
        Assert.Equal(intData, intValue.Value);
        Assert.Equal(stringData, stringValue.Value);

        var firstIdValue = firstSubItem.Value["Id"] as IntegerValue;
        var firstNameValue = firstSubItem.Value["Name"] as StringValue;
        Assert.Equal(firstId, firstIdValue?.Value);
        Assert.Equal(firstName, firstNameValue?.Value);

        var secondIdValue = secondSubItem.Value["Id"] as IntegerValue;
        var secondNameValue = secondSubItem.Value["Name"] as StringValue;
        Assert.Equal(secondId, secondIdValue?.Value);
        Assert.Equal(secondName, secondNameValue?.Value);
    }

    [Fact]
    public void Convert_TestDataArray_IsMappedCorrectToXmlRpcArray()
    {
        const int intData = 1234;
        const string stringData = "TestData";

        const int intData2 = 2468;
        const string stringData2 = "TestData";

        var testData0 = new StructTestDataWithArray
        {
            IntValue = intData,
            StringValue = stringData,
            SubItems = new[]
            {
                new SubTestData
                {
                    Id = 2345,
                    Name = "Item1"
                },
                new SubTestData
                {
                    Id = 3456,
                    Name = "Item2"
                }
            }
        };

        var testData1 = new StructTestDataWithArray
        {
            IntValue = intData2,
            StringValue = stringData2,
            SubItems = new[]
            {
                new SubTestData
                {
                    Id = 45,
                    Name = "Item21"
                },
                new SubTestData
                {
                    Id = 78,
                    Name = "Item22"
                }
            }
        };

        var testData = new[] {testData0, testData1}.ToArray();

        var converter = new DataToXmlRpcValueConverter();

        var convertedValue = converter.Convert(testData);

        Assert.IsType<ArrayValue>(convertedValue);

        var arrayValue = (ArrayValue) convertedValue;

        Assert.Equal(2, arrayValue.Value.Count());

        var firstValue = arrayValue.Value.First();
        var lastValue = arrayValue.Value.Last();

        Assert.IsType<StructValue>(firstValue);
        Assert.IsType<StructValue>(lastValue);

        CheckTestData((StructValue)firstValue, intData, stringData, 2345, "Item1", 3456, "Item2");

        CheckTestData((StructValue)lastValue, intData2, stringData2, 45, "Item21", 78, "Item22");
    }

    [Fact]
    public void Convert_TestDataToXmlRpcAndBack_IsMappedCorrectToObject()
    {
        const int intData = 1234;
        const string stringData = "TestData";

        var testData = new StructTestDataWithArray
        {
            IntValue = intData,
            StringValue = stringData,
            SubItems = new[]
            {
                new SubTestData
                {
                    Id = 2345,
                    Name = "Item1"
                },
                new SubTestData
                {
                    Id = 3456,
                    Name = "Item2"
                }
            }
        };

        var objectToXmlRpcConverter = new DataToXmlRpcValueConverter();

        var xmlRpcToObjectConverter = new XmlRpcValueToDataConverter();

        var xmlRpcValue = objectToXmlRpcConverter.Convert(testData);

        var convertedTestData = xmlRpcToObjectConverter.Convert<StructTestDataWithArray>(xmlRpcValue);

        Assert.Equal(testData.IntValue, convertedTestData.IntValue);
        Assert.Equal(testData.StringValue, convertedTestData.StringValue);
        Assert.Null(convertedTestData.SubData);
        Assert.Equal(testData.SubItems.Length, convertedTestData.SubItems.Length);
        for (var i = 0; i < testData.SubItems.Length; i++)
        {
            Assert.Equal(testData.SubItems[i].Id, convertedTestData.SubItems[i].Id);
            Assert.Equal(testData.SubItems[i].Name, convertedTestData.SubItems[i].Name);
        }
            
    }

    [Fact]
    public void Convert_XmlRpcValueToObjectAndBack_OutputIsEqualToInput()
    {
        var xmlRpcValue = new StructValue();

        xmlRpcValue.Value["IntTest"] = new IntegerValue(987);
        xmlRpcValue.Value["StringTest"] = new StringValue("1234Test");
        xmlRpcValue.Value["SubData"] = new StructValue(new Dictionary<string, XmlRpcValue>
        {
            {"Id", new IntegerValue(4567) }
        });
        xmlRpcValue.Value["SubItems"] = new ArrayValue(new []
        {
            new StructValue(new Dictionary<string, XmlRpcValue>
            {
                {"Id", new IntegerValue(13579)},
                {"Name", new StringValue("SubItem0")}
            }),
            new StructValue(new Dictionary<string, XmlRpcValue>
            {
                {"Id", new IntegerValue(24680)},
                {"Name", new StringValue("SubItem1")}
            })
        });

        var objectToXmlRpcConverter = new DataToXmlRpcValueConverter();

        var xmlRpcToObjectConverter = new XmlRpcValueToDataConverter();

        var obj = xmlRpcToObjectConverter.Convert<StructTestDataWithArray>(xmlRpcValue);

        var convertedXmlRpcValue = objectToXmlRpcConverter.Convert(obj);

        Assert.NotNull(convertedXmlRpcValue);
    }

    [Theory]
    [InlineData(1, TestEnum.One)]
    [InlineData(2, TestEnum.Two)]
    [InlineData(3, TestEnum.Three)]
    [InlineData(4, TestEnum.Four)]
    public void Convert_TestDataWithEnum_IsMappedCorrect(int intEnumValue, TestEnum testEnum)
    {
        var testData = new StructTestDataWithConverter
        {
            TestValue = testEnum
        };

        var objectToXmlRpcConverter = new DataToXmlRpcValueConverter();

        var convertedObject = objectToXmlRpcConverter.Convert(testData);

        Assert.IsType<StructValue>(convertedObject);

        var structValue = (StructValue) convertedObject;

        Assert.Single(structValue.Value);

        var (key, xmlRpcValue) = structValue.Value.First();

        Assert.Equal(nameof(StructTestDataWithConverter.TestValue), key);
        Assert.IsType<IntegerValue>(xmlRpcValue);

        var intValue = (IntegerValue) xmlRpcValue;
        Assert.Equal(intEnumValue, intValue.Value);
    }

    [Theory]
    [InlineData(TestEnum.One)]
    [InlineData(TestEnum.Two)]
    [InlineData(TestEnum.Three)]
    [InlineData(TestEnum.Four)]
    public void Convert_TestDataWithEnumAndBack_IsMappedCorrect(TestEnum testEnum)
    {
        var testData = new StructTestDataWithConverter
        {
            TestValue = testEnum
        };

        var objectToXmlRpcConverter = new DataToXmlRpcValueConverter();

        var xmlRpcValueToObjectConverter = new XmlRpcValueToDataConverter();

        var obj = objectToXmlRpcConverter.Convert(testData);

        var convertedValue = xmlRpcValueToObjectConverter.Convert<StructTestDataWithConverter>(obj);

        Assert.Equal(testEnum, convertedValue.TestValue);
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void Convert_TestDataWithBoolToInteger_IsMappedCorrectToInt(bool boolValue, int intValue)
    {
        var testData = new StructTestDataWithBoolInt
        {
            TestBoolValue = boolValue
        };

        var objectToXmlRpcConverter = new DataToXmlRpcValueConverter();

        var convertedObject = objectToXmlRpcConverter.Convert(testData);

        Assert.IsType<StructValue>(convertedObject);

        var structValue = (StructValue)convertedObject;

        Assert.Single(structValue.Value);

        var (key, xmlRpcValue) = structValue.Value.First();

        Assert.Equal(nameof(StructTestDataWithBoolInt.TestBoolValue), key);
        Assert.IsType<IntegerValue>(xmlRpcValue);

        var integerValue = (IntegerValue)xmlRpcValue;
        Assert.Equal(intValue, integerValue.Value);
    }
}