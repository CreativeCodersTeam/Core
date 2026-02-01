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
using Spectre.Console.Rendering;
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

        SetupServiceProvider(serviceProvider, null);

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(true);

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

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

        SetupServiceProvider(serviceProvider, null);

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(null);

        var groupNode = new CliCommandGroupNode("missing", null);

        A.CallTo(() => commandStore.FindCommandGroupNode(args))
            .Returns(new FindCommandNodeResult<CliCommandGroupNode>(groupNode, []));

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

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

        SetupServiceProvider(serviceProvider, new CliCommandContext());

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

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(5);
    }

    [Theory]
    [InlineData(typeof(DummyCommandWithErrorAbortException), DummyCommandWithErrorAbortException.ExitCode,
        true, DummyCommandWithErrorAbortException.Message, 0)]
    [InlineData(typeof(DummyCommandWithNoneErrorAbortException),
        DummyCommandWithNoneErrorAbortException.ExitCode,
        true, DummyCommandWithNoneErrorAbortException.Message, 1)]
    [InlineData(typeof(DummyCommandWithoutPrintErrorAbortException),
        DummyCommandWithoutPrintErrorAbortException.ExitCode,
        false, DummyCommandWithoutPrintErrorAbortException.Message, 1)]
    public async Task RunAsync_WhenCommandWithAbortException_ExecutesAndReturnsExceptionResult(
        Type commandType, int exitCode, bool shouldPrintMessage, string message, int expectedColor)
    {
        // Arrange
        Color[] colors = [Color.Red, Color.Yellow];
        var args = new[] { "run" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        SetupServiceProvider(serviceProvider, new CliCommandContext());

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = commandType
        };

        var commandNode = new CliCommandNode(commandInfo, "run", null);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(new FindCommandNodeResult<CliCommandNode>(commandNode, []));

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(exitCode);

        if (shouldPrintMessage)
        {
            A.CallTo(() => ansiConsole.Write(A<IRenderable>.That.Matches(r =>
                    CheckConsoleOutput(r, message, colors[expectedColor]))))
                .MustHaveHappenedOnceExactly();
        }
        else
        {
            A.CallTo(() => ansiConsole.Write(A<IRenderable>.Ignored))
                .MustNotHaveHappened();
        }
    }

    private static bool CheckConsoleOutput(IRenderable renderable, string message, Color color)
    {
        var text = string.Join("", renderable.GetSegments(AnsiConsole.Console).Select(s => s.Text));
        var style = renderable.GetSegments(AnsiConsole.Console).First().Style;

        return text.Trim() == message &&
               style.Foreground == color;
    }

    [Fact]
    public async Task RunMainAsync_WhenCommandWithoutOptions_ExecutesAndReturnsIntResult()
    {
        // Arrange
        var args = new[] { "run" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        SetupServiceProvider(serviceProvider, new CliCommandContext());

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

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunMainAsync(args);

        // Assert
        result
            .Should()
            .Be(5);
    }

    [Fact]
    public async Task RunAsync_OnlyArgsForCommand_CommandContextHasCorrectArgs()
    {
        // Arrange
        var args = new[] { "run" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();
        var commandContext = new CliCommandContext();

        SetupServiceProvider(serviceProvider, commandContext);

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

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(5);

        commandContext.AllArgs
            .Should()
            .BeEquivalentTo(args);

        commandContext.OptionsArgs
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task RunAsync_ArgsForCommandAndOptions_CommandContextHasCorrectArgs()
    {
        // Arrange
        var args = new[] { "run", "some", "more" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();
        var commandContext = new CliCommandContext();

        SetupServiceProvider(serviceProvider, commandContext);

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommandWithResult)
        };

        var commandNode = new CliCommandNode(commandInfo, "run", null);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(new FindCommandNodeResult<CliCommandNode>(commandNode, ["some", "more"]));

        A.CallTo(() => serviceProvider.GetService(typeof(int)))
            .Returns(5);

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(5);

        commandContext.AllArgs
            .Should()
            .BeEquivalentTo(args);

        commandContext.OptionsArgs
            .Should()
            .BeEquivalentTo("some", "more");
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

        SetupServiceProvider(serviceProvider, null);

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

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(CliExitCodes.CommandCreationFailed);
    }

    [Fact]
    public async Task RunAsync_WithOptionsValidation_ExecutesAndReturnsResult()
    {
        // Arrange
        var args = new[] { "run" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        SetupServiceProvider(serviceProvider, new CliCommandContext(),
            new CliHostSettings { UseValidation = true });

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommandWithOptions),
            OptionsType = typeof(DummyOptions)
        };

        var commandNode = new CliCommandNode(commandInfo, "run", null);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(new FindCommandNodeResult<CliCommandNode>(commandNode, []));

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(1357);
    }

    [Fact]
    public async Task RunAsync_WithOptionsValidationActivatedButWithoutValidation_ExecutesAndReturnsResult()
    {
        // Arrange
        var args = new[] { "run" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        SetupServiceProvider(serviceProvider, new CliCommandContext(),
            new CliHostSettings { UseValidation = true });

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommandWithoutOptionsValidation),
            OptionsType = typeof(DummyOptionsWithoutValidation)
        };

        var commandNode = new CliCommandNode(commandInfo, "run", null);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(new FindCommandNodeResult<CliCommandNode>(commandNode, []));

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(2468);
    }

    [Fact]
    public async Task RunAsync_WithOptionsValidationIsInvalid_ExecutesAndReturnsInvalidOptionsExitCode()
    {
        // Arrange
        var args = new[] { "run" };

        var ansiConsole = A.Fake<IAnsiConsole>();
        var commandStore = A.Fake<ICliCommandStore>();
        var serviceProvider = A.Fake<IServiceProvider>();
        var helpHandler = A.Fake<ICliCommandHelpHandler>();

        SetupServiceProvider(serviceProvider, new CliCommandContext(),
            new CliHostSettings { UseValidation = true });

        A.CallTo(() => helpHandler.ShouldPrintHelp(args))
            .Returns(false);

        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommandWithInvalidOptions),
            OptionsType = typeof(InvalidDummyOptions)
        };

        var commandNode = new CliCommandNode(commandInfo, "run", null);

        A.CallTo(() => commandStore.FindCommandNode(args))
            .Returns(new FindCommandNodeResult<CliCommandNode>(commandNode, []));

        var host = new DefaultCliHost(ansiConsole, commandStore, serviceProvider, helpHandler, [], []);

        // Act
        var result = await host.RunAsync(args);

        // Assert
        result.ExitCode
            .Should()
            .Be(CliExitCodes.CommandOptionsInvalid);
    }

    private static void SetupServiceProvider(IServiceProvider serviceProvider,
        ICliCommandContext? commandContext,
        CliHostSettings? settings = null)
    {
        if (commandContext != null)
        {
            A.CallTo(() => serviceProvider.GetService(typeof(ICliCommandContext)))
                .Returns(commandContext);
        }

        A.CallTo(() => serviceProvider.GetService(typeof(CliHostSettings)))
            .Returns(settings);
    }

    private sealed class DummyCommandWithOptions : ICliCommand<DummyOptions>
    {
        public Task<CommandResult> ExecuteAsync(DummyOptions options)
        {
            return Task.FromResult(new CommandResult { ExitCode = 1357 });
        }
    }

    [UsedImplicitly]
    private class DummyOptions : IOptionsValidation
    {
        public Task<OptionsValidationResult> ValidateAsync()
        {
            return Task.FromResult(new OptionsValidationResult(true));
        }
    }

    private sealed class DummyCommandWithInvalidOptions : ICliCommand<InvalidDummyOptions>
    {
        public Task<CommandResult> ExecuteAsync(InvalidDummyOptions options)
        {
            return Task.FromResult(new CommandResult { ExitCode = 1357 });
        }
    }

    [UsedImplicitly]
    private class InvalidDummyOptions : IOptionsValidation
    {
        public Task<OptionsValidationResult> ValidateAsync()
        {
            return Task.FromResult(new OptionsValidationResult(false));
        }
    }

    private sealed class DummyCommandWithoutOptionsValidation : ICliCommand<DummyOptionsWithoutValidation>
    {
        public Task<CommandResult> ExecuteAsync(DummyOptionsWithoutValidation options)
        {
            return Task.FromResult(new CommandResult { ExitCode = 2468 });
        }
    }

    [UsedImplicitly]
    private class DummyOptionsWithoutValidation { }

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

    private sealed class DummyCommandWithErrorAbortException : ICliCommand
    {
        public const int ExitCode = 12349876;

        public const string Message = "Command aborted with error";

        public Task<CommandResult> ExecuteAsync()
        {
            throw new CliCommandAbortException(Message, ExitCode);
        }
    }

    private sealed class DummyCommandWithNoneErrorAbortException : ICliCommand
    {
        public const int ExitCode = 12349876;

        public const string Message = "Command aborted with warning";

        public Task<CommandResult> ExecuteAsync()
        {
            throw new CliCommandAbortException(Message, ExitCode)
            {
                IsError = false
            };
        }
    }

    private sealed class DummyCommandWithoutPrintErrorAbortException : ICliCommand
    {
        public const int ExitCode = 12349876;

        public const string Message = "Command aborted with printing";

        public Task<CommandResult> ExecuteAsync()
        {
            throw new CliCommandAbortException(Message, ExitCode)
            {
                PrintMessage = false
            };
        }
    }
}
