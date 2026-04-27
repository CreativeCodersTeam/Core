using AwesomeAssertions;
using CreativeCoders.Cli.Commands.Output;
using Spectre.Console.Testing;
using Xunit;

namespace CreativeCoders.Cli.Commands.UnitTests.Output;

public class PlainOutputFormatterTests
{
    [Fact]
    public async Task WriteAsync_Sequence_OneLinePerItem()
    {
        var sut = new PlainOutputFormatter<IReadOnlyList<string>>();
        var console = new TestConsole();

        await sut.WriteAsync(new[] { "alpha", "beta", "gamma" }, console, CancellationToken.None);

        var lines = console.Output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        lines.Should().Equal("alpha", "beta", "gamma");
    }

    [Fact]
    public async Task WriteAsync_Scalar_WritesToString()
    {
        var sut = new PlainOutputFormatter<int>();
        var console = new TestConsole();

        await sut.WriteAsync(42, console, CancellationToken.None);

        console.Output.Trim().Should().Be("42");
    }

    [Fact]
    public async Task WriteAsync_String_TreatedAsScalar()
    {
        var sut = new PlainOutputFormatter<string>();
        var console = new TestConsole();

        await sut.WriteAsync("hello", console, CancellationToken.None);

        console.Output.Trim().Should().Be("hello");
    }
}
