using AwesomeAssertions;
using CreativeCoders.ProcessUtils.Execution.Parsers;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Execution.Parsers;

/// <summary>
///     Tests for <see cref="PassThroughProcessOutputParser"/> ensuring the parser returns the input unchanged.
/// </summary>
public class PassThroughProcessOutputParserTests
{
    /// <summary>
    ///     Ensures that <see cref="PassThroughProcessOutputParser.ParseOutput"/> returns the same instance for null input.
    /// </summary>
    [Fact]
    public void ParseOutput_WithNull_ReturnsNull()
    {
        // Arrange
        var parser = new PassThroughProcessOutputParser();

        // Act
        var result = parser.ParseOutput(null);

        // Assert
        result
            .Should()
            .BeNull();
    }

    /// <summary>
    ///     Ensures that empty strings are returned unchanged.
    /// </summary>
    [Fact]
    public void ParseOutput_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var parser = new PassThroughProcessOutputParser();

        // Act
        var result = parser.ParseOutput(string.Empty);

        // Assert
        result
            .Should()
            .Be("");
    }

    /// <summary>
    ///     Ensures that non-empty strings are returned unchanged.
    /// </summary>
    [Fact]
    public void ParseOutput_WithText_ReturnsSameText()
    {
        // Arrange
        var parser = new PassThroughProcessOutputParser();
        const string input = "some output";

        // Act
        var result = parser.ParseOutput(input);

        // Assert
        result
            .Should()
            .Be(input);
    }
}
