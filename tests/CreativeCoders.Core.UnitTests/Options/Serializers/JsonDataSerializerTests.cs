using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using CreativeCoders.Options.Serializers;
using AwesomeAssertions;
using Xunit;

#nullable enable

namespace CreativeCoders.Core.UnitTests.Options.Serializers;

[SuppressMessage("Performance", "CA1869", Justification = "Just test code")]
[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed", Justification = "Just test code")]
public class JsonDataSerializerTests
{
    [Fact]
    public void Serialize_ObjectGiven_OutputsCorrectJson()
    {
        // Arrange
        var data = new TestOptions
        {
            IntValue = 10,
            IsEnabled = true,
            Name = "abcd"
        };

        var serializer = new JsonDataSerializer<TestOptions>();

        // Act
        var json = serializer.Serialize(data);

        // Assert
        json
            .Should()
            .Be(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
    }

    [Fact]
    public void Serialize_JsonSerializerOptionsGivenViaCtor_OutputsCorrectJson()
    {
        // Arrange
        var data = new TestOptions
        {
            IntValue = 10,
            IsEnabled = true,
            Name = "abcd"
        };

        var serializer =
            new JsonDataSerializer<TestOptions>(new JsonSerializerOptions { WriteIndented = false });

        // Act
        var json = serializer.Serialize(data);

        // Assert
        json
            .Should()
            .Be(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = false }));
    }

    [Fact]
    public void Deserialize_JsonGiven_DeserializesCorrectObject()
    {
        // Arrange
        var data = new TestOptions
        {
            IntValue = 10,
            IsEnabled = true,
            Name = "abcd"
        };

        var serializer = new JsonDataSerializer<TestOptions>();

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

        var deserializedData = new TestOptions();

        // Act
        serializer.Deserialize(json, deserializedData);

        // Assert
        deserializedData
            .Should()
            .BeEquivalentTo(data);
    }

    [Fact]
    public void Deserialize_OnlySomePropertiesAreSetInJson_OnlyPropertiesFromJsonAreUpdatedOnObject()
    {
        // Arrange
        var serializer = new JsonDataSerializer<TestOptions>();

        const string json = "{\"IntValue\": 20}";

        var deserializedData = new TestOptions
        {
            IntValue = 10,
            IsEnabled = false,
            Name = "abcd"
        };

        // Act
        serializer.Deserialize(json, deserializedData);

        // Assert
        deserializedData
            .Should()
            .BeEquivalentTo(new TestOptions
            {
                IntValue = 20,
                IsEnabled = false,
                Name = "abcd"
            });
    }

    [Fact]
    public void Deserialize_EmptyJsonString_DoesNotChangeObject()
    {
        // Arrange
        const string json = "";

        var serializer = new JsonDataSerializer<TestOptions>();

        var deserializedData = new TestOptions
        {
            IntValue = 10,
            IsEnabled = false,
            Name = "abcd"
        };

        // Act
        serializer.Deserialize(json!, deserializedData);

        // Assert
        deserializedData
            .Should()
            .BeEquivalentTo(new TestOptions
            {
                IntValue = 10,
                IsEnabled = false,
                Name = "abcd"
            });
    }
}
