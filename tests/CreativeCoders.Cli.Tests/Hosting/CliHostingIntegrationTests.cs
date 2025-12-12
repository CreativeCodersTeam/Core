using System.Reflection;
using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting;
using CreativeCoders.Cli.Hosting.Help;
using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Xunit;

// ReSharper disable once CheckNamespace
[assembly: CliCommandGroup(["int"], "int: integration commands")]

namespace CreativeCoders.Cli.Tests.Hosting;

public class CliHostingIntegrationTests
{
    [Fact]
    public async Task RunAsync_WhenHelpIsRequested_PrintsHelpAndReturnsSuccess()
    {
        // Arrange
        var host = CreateHostWithOutput(out var output);

        // Act
        var result = await host.RunAsync(["help"]);

        // Assert
        result.ExitCode
            .Should()
            .Be(CliExitCodes.Success);

        output.ToString()
            .Should()
            .Contain("int: integration commands");
    }

    [Fact]
    public async Task RunAsync_WhenCommandExecutes_ReturnsCommandExitCode()
    {
        // Arrange
        var host = CreateHostWithOutput(out var output);

        // Act
        var result = await host.RunAsync(["int", "simple"]);

        // Assert
        result.ExitCode
            .Should()
            .Be(42);

        output.ToString()
            .Should()
            .Contain("simple executed");
    }

    [Fact]
    public async Task RunAsync_WhenCommandWithOptions_ParsesOptionsAndOutputsData()
    {
        // Arrange
        var host = CreateHostWithOutput(out var output);

        // Act
        var result = await host.RunAsync(["int", "echo", "hello", "--times", "2"]);

        // Assert
        result.ExitCode
            .Should()
            .Be(CliExitCodes.Success);

        output.ToString()
            .Should()
            .Contain("hello")
            .And
            .Contain("Repeat: 2");
    }

    [Fact]
    public async Task RunAsync_WhenCommandNotFound_PrintsSuggestionsAndReturnsError()
    {
        // Arrange
        var host = CreateHostWithOutput(out var output);

        // Act
        var result = await host.RunAsync(["int", "missing"]);

        // Assert
        result.ExitCode
            .Should()
            .Be(CliExitCodes.CommandNotFound);

        var outputText = output.ToString();

        outputText
            .Should()
            .Contain("No command found for given arguments")
            .And
            .Contain("Suggestions:")
            .And
            .Contain("echo");
    }

    [Fact]
    public async Task RunAsync_WhenCommandConstructionFails_ReturnsConstructionFailedExitCode()
    {
        // Arrange
        var host = CreateHostWithOutput(out var output);

        // Act
        var result = await host.RunAsync(["int", "fail"]);

        // Assert
        result.ExitCode
            .Should()
            .Be(CliExitCodes.CommandCreationFailed);

        output.ToString()
            .Should()
            .Contain("Error creating command");
    }

    private static ICliHost CreateHostWithOutput(out StringWriter output)
    {
        var writer = new StringWriter();
        Console.SetOut(writer);
        output = writer;

        return CliHostBuilder.Create()
            .SkipScanEntryAssembly()
            .ScanAssemblies(Assembly.GetExecutingAssembly())
            .EnableHelp(HelpCommandKind.Command)
            .ConfigureServices(services =>
            {
                services.AddSingleton<IAnsiConsole>(_ =>
                    AnsiConsole.Create(new AnsiConsoleSettings
                    {
                        Out = new AnsiConsoleOutput(writer)
                    }));
            })
            .Build();
    }

    [UsedImplicitly]
    [CliCommand(["int", "simple"], Description = "simple command")]
    private sealed class SimpleCommand(IAnsiConsole ansiConsole) : ICliCommand
    {
        public Task<CommandResult> ExecuteAsync()
        {
            ansiConsole.WriteLine("simple executed");

            return Task.FromResult(new CommandResult { ExitCode = 42 });
        }
    }

    [UsedImplicitly]
    [CliCommand(["int", "echo"], Description = "echo command")]
    private sealed class EchoCommand(IAnsiConsole ansiConsole) : ICliCommand<EchoOptions>
    {
        public Task<CommandResult> ExecuteAsync(EchoOptions options)
        {
            ansiConsole.WriteLine(options.Text ?? string.Empty);
            ansiConsole.WriteLine($"Repeat: {options.Times}");

            return Task.FromResult(new CommandResult { ExitCode = CliExitCodes.Success });
        }
    }

    /// <summary>
    ///     Options for <see cref="EchoCommand"/>.
    /// </summary>
    [PublicAPI]
    public sealed class EchoOptions
    {
        [OptionValue(0, HelpText = "Text to echo")]
        public string? Text { get; set; }

        [OptionParameter('t', "times", HelpText = "Number of repetitions", DefaultValue = 1)]
        public int Times { get; set; }
    }

    [UsedImplicitly]
    [CliCommand(["int", "fail"], Description = "failing command")]
    private sealed class FailingCommand : ICliCommand
    {
        [UsedImplicitly]
        public FailingCommand()
        {
            throw new InvalidOperationException("Failing command ctor");
        }

        public Task<CommandResult> ExecuteAsync()
        {
            return Task.FromResult(new CommandResult());
        }
    }
}
