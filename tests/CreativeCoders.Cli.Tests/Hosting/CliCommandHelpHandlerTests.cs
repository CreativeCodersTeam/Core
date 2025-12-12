using System.Diagnostics.CodeAnalysis;
using System.Text;
using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Help;
using CreativeCoders.SysConsole.Cli.Parsing.Help;
using FakeItEasy;
using JetBrains.Annotations;
using Spectre.Console;
using Xunit;

namespace CreativeCoders.Cli.Tests.Hosting;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class CliCommandHelpHandlerTests
{
    [Fact]
    public void ShouldPrintHelp_CommandKindCommand_RespectsHelpCommand()
    {
        // Arrange
        var handler = CreateHandler(HelpCommandKind.Command, out var stringWriter, out _);

        // Act
        var resultHelp = handler.ShouldPrintHelp(["help"]);
        var resultOther = handler.ShouldPrintHelp(["run"]);

        // Assert
        resultHelp
            .Should()
            .BeTrue();

        resultOther
            .Should()
            .BeFalse();

        stringWriter.Dispose();
    }

    [Fact]
    public void ShouldPrintHelp_CommandKindArgument_RespectsHelpArgument()
    {
        // Arrange
        var handler = CreateHandler(HelpCommandKind.Argument, out var stringWriter, out _);

        // Act
        var resultHelp = handler.ShouldPrintHelp(["run", "--help"]);
        var resultOther = handler.ShouldPrintHelp(["run", "--verbose"]);

        // Assert
        resultHelp
            .Should()
            .BeTrue();

        resultOther
            .Should()
            .BeFalse();

        stringWriter.Dispose();
    }

    [Fact]
    public void PrintHelp_WhenCommandFound_PrintsDescriptionSyntaxAndOptions()
    {
        // Arrange
        var commandAttribute = new CliCommandAttribute(["test", "run"])
        {
            Name = "test run",
            Description = "Runs the test command",
            AlternativeCommands = ["tr"]
        };

        var optionsHelp = new OptionsHelp(
            [new HelpEntry { ArgumentName = "value", HelpText = "A value" }],
            [new HelpEntry { ArgumentName = "--flag", HelpText = "A flag" }]);

        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = commandAttribute,
            CommandType = typeof(DummyCommand),
            OptionsType = typeof(DummyOptions)
        };

        var commandStore = new CliCommandStore();
        commandStore.AddCommands([commandInfo]);

        var helpHandler = CreateHandler(HelpCommandKind.CommandOrArgument, out var writer,
            out var optionsHelpGenerator, commandStore);

        A.CallTo(() => optionsHelpGenerator.CreateHelp(typeof(DummyOptions)))
            .Returns(optionsHelp);

        // Act
        helpHandler.PrintHelp(["help", "test", "run"]);

        // Assert
        var output = writer.ToString();

        output
            .Should()
            .Contain("test run")
            .And
            .Contain("Description:")
            .And
            .Contain("Runs the test command")
            .And
            .Contain("Syntax:")
            .And
            .Contain("test run")
            .And
            .Contain("<value>")
            .And
            .Contain("A value")
            .And
            .Contain("Options:")
            .And
            .Contain("--flag")
            .And
            .Contain("A flag");

        A.CallTo(() => optionsHelpGenerator.CreateHelp(typeof(DummyOptions)))
            .MustHaveHappenedOnceExactly();

        writer.Dispose();
    }

    [Fact]
    public void PrintHelp_WhenRootRequested_PrintsGroupsAndCommands()
    {
        // Arrange
        var commandInfoOne = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["group", "one"])
            {
                Name = "group one",
                Description = "First command"
            },
            CommandType = typeof(DummyCommand)
        };

        var commandInfoTwo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["group", "two"])
            {
                Name = "group two",
                Description = "Second command"
            },
            CommandType = typeof(DummyCommand)
        };

        var commandStore = new CliCommandStore();
        commandStore.AddCommands([commandInfoOne, commandInfoTwo]);

        var handler = CreateHandler(HelpCommandKind.CommandOrArgument, out var writer, out _, commandStore);

        // Act
        handler.PrintHelp([]);

        // Assert
        var output = writer.ToString();

        output
            .Should()
            .Contain("Groups:")
            .And
            .Contain("group");

        writer.Dispose();
    }

    private static CliCommandHelpHandler CreateHandler(
        HelpCommandKind commandKind,
        out StringWriter writer,
        out IOptionsHelpGenerator optionsHelpGenerator,
        ICliCommandStore? commandStore = null)
    {
        writer = new StringWriter(new StringBuilder());

        var ansiConsole = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer)
        });

        var settings = new HelpHandlerSettings { CommandKind = commandKind };
        optionsHelpGenerator = A.Fake<IOptionsHelpGenerator>();
        var store = commandStore ?? new CliCommandStore();

        return new CliCommandHelpHandler(settings, store, ansiConsole, optionsHelpGenerator);
    }

    private sealed class DummyCommand : ICliCommand
    {
        public Task<CommandResult> ExecuteAsync()
        {
            return Task.FromResult(new CommandResult());
        }
    }

    [UsedImplicitly]
    private sealed class DummyOptions { }
}
