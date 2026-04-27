using AwesomeAssertions;
using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Options;
using CreativeCoders.Cli.Commands.Output;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Testing;
using Xunit;

namespace CreativeCoders.Cli.Commands.UnitTests;

public class ServiceCollectionExtensionsTests
{
    private sealed record User(string Name);

    [Fact]
    public void AddCliCommandsBaseClasses_RegistersAllDefaultServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IAnsiConsole>(new TestConsole());

        services.AddCliCommandsBaseClasses();

        using var sp = services.BuildServiceProvider();

        sp.GetService<IConfirmationPrompt>().Should().BeOfType<SpectreConfirmationPrompt>();
        sp.GetService<IInteractivePrompter>().Should().BeOfType<SpectreInteractivePrompter>();

        var formatters = sp.GetServices<IOutputFormatter<User>>().ToList();
        formatters.Should().HaveCount(3);
        formatters.Select(f => f.Format).Should()
            .BeEquivalentTo(new[] { OutputFormat.Json, OutputFormat.Table, OutputFormat.Plain });
    }
}
