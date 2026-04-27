using AwesomeAssertions;
using CreativeCoders.Cli.Commands.Output;
using Spectre.Console.Testing;
using Xunit;

namespace CreativeCoders.Cli.Commands.UnitTests.Output;

public class TableOutputFormatterTests
{
    private sealed record User(string Name, int Age);

    [Fact]
    public async Task WriteAsync_Sequence_RendersColumnsAndRows()
    {
        var sut = new TableOutputFormatter<IReadOnlyList<User>>();
        var console = new TestConsole();

        await sut.WriteAsync(
            new[] { new User("alice", 30), new User("bob", 25) },
            console, CancellationToken.None);

        console.Output.Should().Contain("Name").And.Contain("Age");
        console.Output.Should().Contain("alice").And.Contain("bob");
    }

    [Fact]
    public async Task WriteAsync_EmptySequence_RendersNoItemsMessage()
    {
        var sut = new TableOutputFormatter<IReadOnlyList<User>>();
        var console = new TestConsole();

        await sut.WriteAsync(Array.Empty<User>(), console, CancellationToken.None);

        console.Output.Should().Contain("no items");
    }

    [Fact]
    public async Task WriteAsync_NullValue_DoesNotThrow()
    {
        var sut = new TableOutputFormatter<User?>();
        var console = new TestConsole();

        await sut.WriteAsync(null, console, CancellationToken.None);

        console.Output.Should().BeEmpty();
    }
}
