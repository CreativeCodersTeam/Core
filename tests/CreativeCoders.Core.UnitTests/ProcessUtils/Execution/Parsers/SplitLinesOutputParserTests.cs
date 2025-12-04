using System;
using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils.Execution.Parsers;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Execution.Parsers;

/// <summary>
///     Tests for <see cref="SplitLinesOutputParser"/> to ensure line splitting and empty handling behave correctly.
/// </summary>
[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class SplitLinesOutputParserTests
{
    /// <summary>
    ///     Returns an empty array if the input is null.
    /// </summary>
    [Fact]
    public void ParseOutput_WithNull_ReturnsEmptyArray()
    {
        // Arrange
        var parser = new SplitLinesOutputParser();

        // Act
        var result = parser.ParseOutput(null);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!
            .Length
            .Should()
            .Be(0);
    }

    /// <summary>
    ///     Returns an empty array if the input is an empty string.
    /// </summary>
    [Fact]
    public void ParseOutput_WithEmpty_ReturnsEmptyArray()
    {
        // Arrange
        var parser = new SplitLinesOutputParser();

        // Act
        var result = parser.ParseOutput(string.Empty);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!
            .Length
            .Should()
            .Be(0);
    }

    /// <summary>
    ///     Splits a single line into a single array element.
    /// </summary>
    [Fact]
    public void ParseOutput_WithSingleLine_ReturnsSingleElement()
    {
        // Arrange
        var parser = new SplitLinesOutputParser();
        const string line = "hello";

        // Act
        var result = parser.ParseOutput(line);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!
            .Length
            .Should()
            .Be(1);

        result[0]
            .Should()
            .Be(line);
    }

    /// <summary>
    ///     Splits multiple lines using <see cref="Environment.NewLine"/> and removes empty entries.
    /// </summary>
    [Fact]
    public void ParseOutput_WithMultipleLines_RemovesEmptyEntriesAndSplits()
    {
        // Arrange
        var parser = new SplitLinesOutputParser()
        {
            SplitOptions = StringSplitOptions.RemoveEmptyEntries
        };

        var nl = Environment.NewLine;
        string[] lines = ["line1", string.Empty, "line2", "line3", string.Empty];
        var input = string.Join(nl, lines);

        // Act
        var result = parser.ParseOutput(input);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!
            .Should()
            .HaveCount(3)
            .And
            .BeEquivalentTo("line1", "line2", "line3");
    }

    /// <summary>
    ///     Splits multiple lines using <see cref="Environment.NewLine"/> and removes empty entries.
    /// </summary>
    [Fact]
    public void ParseOutput_WithMultipleLines_DontRemovesEmptyEntriesAndSplits()
    {
        // Arrange
        var parser = new SplitLinesOutputParser();

        var nl = Environment.NewLine;
        var input = string.Join(nl, "line1", string.Empty, "line2", "line3", string.Empty);

        // Act
        var result = parser.ParseOutput(input);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!
            .Should()
            .HaveCount(5)
            .And
            .BeEquivalentTo("line1", string.Empty, "line2", "line3", string.Empty);
    }

    /// <summary>
    ///     Splits multiple lines using <see cref="Environment.NewLine"/> and removes empty entries.
    /// </summary>
    [Fact]
    public void ParseOutput_WithMultipleLines_SplitAndTrimLinesAndRemovesEmptyEntries()
    {
        // Arrange
        var parser = new SplitLinesOutputParser()
        {
            SplitOptions = StringSplitOptions.RemoveEmptyEntries,
            TrimLines = true
        };

        var nl = Environment.NewLine;
        var input = string.Join(nl, "  line1", string.Empty, "line2  ", "  \tline3  ", string.Empty);

        // Act
        var result = parser.ParseOutput(input);

        // Assert
        result
            .Should()
            .NotBeNull();

        result!
            .Should()
            .HaveCount(3)
            .And
            .BeEquivalentTo("line1", "line2", "line3");
    }
}
