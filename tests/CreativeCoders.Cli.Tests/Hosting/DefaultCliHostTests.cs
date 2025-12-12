using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Exceptions;
using CreativeCoders.Cli.Hosting.Help;
using FakeItEasy;
using JetBrains.Annotations;
using Spectre.Console;
using Xunit;

namespace CreativeCoders.Cli.Tests.Hosting;

public class DefaultCliHostTests
{
    [Fact]
    public async Task RunAsync_WhenHelpIsRequested_PrintsHelpAndReturnsSuccess()
    {
        // Arrange
        var args = new[] { "help" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(true);

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        A.CallTo(() => helpHandler.PrintHelp(args))
            .MustHaveHappenedOnceExactly();

        result.ExitCode
            .Should()
            .Be(CliExitCodes.Success);
    }

    [Fact]
    public async Task RunAsync_WhenCommandNotFound_PrintsSuggestionsAndReturnsNotFound()
    {
        // Arrange
        var args = new[] { "missing" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(null);

        var groupNode = new CliCommandGroupNode("missing", null);

        A.CallTo(() => commandStore.FindCommandGroupNode(args))
            .Returns(new FindCommandNodeResult<CliCommandGroupNode>(groupNode, []));

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(CliExitCodes.CommandNotFound);

        A.CallTo(() => helpHandler.PrintHelpFor(A<IList<CliTreeNode>>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RunAsync_WhenCommandWithoutOptions_ExecutesAndReturnsResult()
    {
        // Arrange
        var args = new[] { "run" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommandWithResult)
        };

        var commandNode = new CliCommandNode(commandInfo, "run", null);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(new FindCommandNodeResult<CliCommandNode>(commandNode, []));

        A.CallTo(() => serviceProvider.GetService(typeof(int)))
            .Returns(5);

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(5);
    }

    [Fact]
    public async Task RunAsync_WhenCommandCreationFails_PrintsErrorAndReturnsExitCode()
    {
        // Arrange
        var args = new[] { "run" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(FailingCommand)
        };

        var commandNode = new CliCommandNode(commandInfo, "run", null);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(new FindCommandNodeResult<CliCommandNode>(commandNode, []));

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(CliExitCodes.CommandCreationFailed);
    }

    [UsedImplicitly]
    private sealed class DummyCommand : ICliCommand
    {
        [UsedImplicitly] public CommandResult? Result { get; set; }

        public Task<CommandResult> ExecuteAsync()
        {
            return Task.FromResult(Result ?? new CommandResult());
        }
    }

    [method: UsedImplicitly]
    private sealed class DummyCommandWithResult(int exitCode) : ICliCommand
    {
        public Task<CommandResult> ExecuteAsync()
        {
            return Task.FromResult(new CommandResult { ExitCode = exitCode });
        }
    }

    private sealed class FailingCommand : ICliCommand
    {
        [UsedImplicitly]
        public FailingCommand()
        {
            throw new InvalidOperationException("Failure in constructor");
        }

        public Task<CommandResult> ExecuteAsync()
        {
            return Task.FromResult(new CommandResult());
        }
    }
}
