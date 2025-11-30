using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils.Execution.Parsers;
using JetBrains.Annotations;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Parsers;

/// <summary>
///     Tests for <see cref="JsonOutputParser{T}"/> verifying JSON deserialization behavior and edge cases.
/// </summary>
[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class JsonOutputParserTests
{
    [UsedImplicitly]
    private sealed record Person(string Name, int Age);

    /// <summary>
    ///     Returns default(T) for null input.
    /// </summary>
    [Fact]
    public void ParseOutput_WithNull_ReturnsDefault()
    {
        // Arrange
        var parser = new JsonOutputParser<Person>();

        // Act
        var result = parser.ParseOutput(null);

        // Assert
        result
            .Should()
            .BeNull();
    }

    /// <summary>
    ///     Returns default(T) for whitespace input.
    /// </summary>
    [Fact]
    public void ParseOutput_WithWhitespace_ReturnsDefault()
    {
        // Arrange
        var parser = new JsonOutputParser<Person>();

        // Act
        var result = parser.ParseOutput("   \t\n");

        // Assert
        result
            .Should()
            .BeNull();
    }

    /// <summary>
    ///     Deserializes a valid JSON string to the requested type.
    /// </summary>
    [Fact]
    public void ParseOutput_WithValidJson_ReturnsDeserializedObject()
    {
        // Arrange
        var parser = new JsonOutputParser<Person>();
        const string json = "{\"Name\":\"Alice\",\"Age\":30}";

        // Act
        var result = parser.ParseOutput(json);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!.Name
            .Should()
            .Be("Alice");

        result.Age
            .Should()
            .Be(30);
    }

    /// <summary>
    ///     Throws <see cref="JsonException"/> for invalid JSON input.
    /// </summary>
    [Fact]
    public void ParseOutput_WithInvalidJson_ThrowsJsonException()
    {
        // Arrange
        var parser = new JsonOutputParser<Person>();
        const string invalidJson = "{\"Name\":\"Alice\",\"Age\": }"; // malformed

        // Act
        var act = () => parser.ParseOutput(invalidJson);

        // Assert
        Assert.Throws<JsonException>(act);
    }
}
