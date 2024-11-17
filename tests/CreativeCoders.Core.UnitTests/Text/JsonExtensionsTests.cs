using System.Text.Json;
using CreativeCoders.Core.Text;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Text;

public class JsonExtensionsTests
{
    [Fact]
    public void PopulateJson_JsonDataWithSomeProperties_PropertiesAreSet()
    {
        // Arrange
        const string expectedStrValue = "Test";
        const int expectedIntValue = 1357;

        var jsonData = $"{{\"StrValue\":\"{expectedStrValue}\",\"IntValue\":{expectedIntValue}}}";
        var testObject = new TestClass
        {
            FloatValue = 123,
            BoolValue = true
        };

        // Act
        jsonData.PopulateJson(testObject);

        // Assert
        testObject.StrValue
            .Should()
            .Be(expectedStrValue);

        testObject.IntValue
            .Should()
            .Be(expectedIntValue);

        testObject.BoolValue
            .Should()
            .BeTrue();

        testObject.FloatValue
            .Should()
            .Be(123);
    }

    [Fact]
    public void PopulateJson_JsonDataWithCamelCaseProperties_PropertiesAreSet()
    {
        // Arrange
        const string expectedStrValue = "Test";
        const int expectedIntValue = 1357;

        var jsonData = $"{{\"strValue\":\"{expectedStrValue}\",\"intValue\":{expectedIntValue}}}";
        var testObject = new TestClass();

        // Act
        jsonData.PopulateJson(testObject,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        // Assert
        testObject.StrValue
            .Should()
            .Be(expectedStrValue);

        testObject.IntValue
            .Should()
            .Be(expectedIntValue);
    }
}
