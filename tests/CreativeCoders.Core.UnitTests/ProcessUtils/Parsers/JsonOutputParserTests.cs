using System.Text.Json;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils.Execution.Parsers;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Parsers;

/// <summary>
///     Tests for <see cref="JsonOutputParser{T}"/> verifying JSON deserialization behavior and edge cases.
/// </summary>
public class JsonOutputParserTests
{
    private sealed record Person(string Name, int Age);

    /// <summary>
    ///     Returns default(T) for null input.
    /// </summary>
    [Fact]
    public void ParseOutput_WithNull_ReturnsDefault()
    {
        var parser = new JsonOutputParser<Person>();

        var result = parser.ParseOutput(null);

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
        var parser = new JsonOutputParser<Person>();

        var result = parser.ParseOutput("   \t\n");

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
        var parser = new JsonOutputParser<Person>();
        const string json = "{\"Name\":\"Alice\",\"Age\":30}";

        var result = parser.ParseOutput(json);

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
        var parser = new JsonOutputParser<Person>();
        const string invalidJson = "{\"Name\":\"Alice\",\"Age\": }"; // malformed

        var act = () => parser.ParseOutput(invalidJson);

        Assert.Throws<JsonException>(() => act());
    }
}
