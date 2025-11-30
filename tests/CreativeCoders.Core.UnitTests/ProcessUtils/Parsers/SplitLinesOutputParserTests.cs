using System;
using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using CreativeCoders.ProcessUtils.Execution.Parsers;
using Xunit;

namespace CreativeCoders.Core.UnitTests.ProcessUtils.Parsers;

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
        var parser = new SplitLinesOutputParser();

        var result = parser.ParseOutput(null);

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
        var parser = new SplitLinesOutputParser();

        var result = parser.ParseOutput(string.Empty);

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
        var parser = new SplitLinesOutputParser();
        const string line = "hello";

        var result = parser.ParseOutput(line);

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
        var parser = new SplitLinesOutputParser()
        {
            SplitOptions = StringSplitOptions.RemoveEmptyEntries
        };

        var nl = Environment.NewLine;
        var input = string.Join(nl, "line1", string.Empty, "line2", "line3", string.Empty);

        var result = parser.ParseOutput(input);

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
        var parser = new SplitLinesOutputParser();

        var nl = Environment.NewLine;
        var input = string.Join(nl, "line1", string.Empty, "line2", "line3", string.Empty);

        var result = parser.ParseOutput(input);

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
        var parser = new SplitLinesOutputParser()
        {
            SplitOptions = StringSplitOptions.RemoveEmptyEntries,
            TrimLines = true
        };

        var nl = Environment.NewLine;
        var input = string.Join(nl, "  line1", string.Empty, "line2  ", "  \tline3  ", string.Empty);

        var result = parser.ParseOutput(input);

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
