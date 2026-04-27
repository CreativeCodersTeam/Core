using AwesomeAssertions;
using CreativeCoders.Cli.Commands.Options;
using CreativeCoders.Cli.Commands.Output;
using Spectre.Console.Testing;
using Xunit;

namespace CreativeCoders.Cli.Commands.UnitTests.Output;

public class JsonOutputFormatterTests
{
    private sealed record User(string Name, int Age);

    [Fact]
    public async Task WriteAsync_RendersCamelCaseIndentedJson()
    {
        var sut = new JsonOutputFormatter<User>();
        var console = new TestConsole();

        await sut.WriteAsync(new User("alice", 30), console, CancellationToken.None);

        console.Output.Should().Contain("\"name\": \"alice\"");
        console.Output.Should().Contain("\"age\": 30");
    }

    [Fact]
    public void Format_IsJson()
    {
        new JsonOutputFormatter<User>().Format.Should().Be(OutputFormat.Json);
    }
}
