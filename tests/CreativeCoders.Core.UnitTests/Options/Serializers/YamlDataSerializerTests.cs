using System;
using CreativeCoders.Options.Serializers;
using AwesomeAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Options.Serializers;

public class YamlDataSerializerTests
{
    [Fact]
    public void Serialize_ObjectGiven_OutputsCorrectYaml()
    {
        // Arrange
        var data = new TestOptions
        {
            Name = "Test",
            IsEnabled = true,
            IntValue = 42
        };

        var serializer = new YamlDataSerializer<TestOptions>();

        // Act
        var result = serializer.Serialize(data);

        // Assert
        result
            .Should()
            .Be(
                $"Name: Test{Environment.NewLine}IntValue: 42{Environment.NewLine}IsEnabled: true{Environment.NewLine}");
    }

    [Fact]
    public void Deserialize_YamlGiven_DeserializesCorrectObject()
    {
        // Arrange
        var yaml =
            $"Name: Test{Environment.NewLine}IntValue: 42{Environment.NewLine}IsEnabled: true{Environment.NewLine}";

        var serializer = new YamlDataSerializer<TestOptions>();

        var options = new TestOptions();

        // Act
        serializer.Deserialize(yaml, options);

        // Assert
        options.Name
            .Should()
            .Be("Test");

        options.IntValue
            .Should()
            .Be(42);

        options.IsEnabled
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Deserialize_OnlySomePropertiesAreSetInYaml_OnlyPropertiesFromJsonAreUpdatedOnObject()
    {
        // Arrange
        var yaml = $"Name: Test{Environment.NewLine}";

        var serializer = new YamlDataSerializer<TestOptions>();

        var options = new TestOptions
        {
            Name = "InitialName",
            IntValue = 42,
            IsEnabled = true
        };

        // Act
        serializer.Deserialize(yaml, options);

        // Assert
        options.Name
            .Should()
            .Be("Test");

        options.IntValue
            .Should()
            .Be(42);

        options.IsEnabled
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Deserialize_EmptyYamlString_DoesNotChangeObject()
    {
        // Arrange
        var yaml = string.Empty;

        var serializer = new YamlDataSerializer<TestOptions>();

        var options = new TestOptions
        {
            Name = "InitialName",
            IntValue = 42,
            IsEnabled = true
        };

        // Act
        serializer.Deserialize(yaml, options);

        // Assert
        options.Name
            .Should()
            .Be("InitialName");

        options.IntValue
            .Should()
            .Be(42);

        options.IsEnabled
            .Should()
            .BeTrue();
    }
}
