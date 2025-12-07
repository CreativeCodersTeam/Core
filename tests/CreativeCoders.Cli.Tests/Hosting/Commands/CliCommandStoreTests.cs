using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using JetBrains.Annotations;
using Xunit;

namespace CreativeCoders.Cli.Tests.Hosting.Commands;

public class CliCommandStoreTests
{
    [Fact]
    public void AddCommands_WithSingleCommand_AddsRootCommandNode()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();

        // Act
        store.AddCommands([cmdInfo]);

        // Assert
        store.Commands
            .Should()
            .Contain(cmdInfo);

        var rootNodes = store.TreeRootNodes.ToArray();

        rootNodes
            .Should()
            .HaveCount(1);

        var node = rootNodes[0]
            .Should()
            .BeOfType<CliCommandNode>()
            .Which;

        node.Name
            .Should()
            .Be("run");

        node.CommandInfo
            .Should()
            .BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandForArgs_WithSingleCommand_ReturnsCommandInfo()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandForArgs(["run"]);

        // Assert
        result
            .Should()
            .BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandForArgs_WithMultiPartCommand_ReturnsCommandInfo()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "the", "command"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandForArgs(["run", "the", "command"]);

        // Assert
        result
            .Should()
            .BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandForArgs_WithMultiPartCommandAndExtraArgs_ReturnsCommandInfo()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "the", "command"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandForArgs(["run", "the", "command", "extra", "args"]);

        // Assert
        result
            .Should()
            .BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandForArgs_MultipleCommandsInSameGroup_ReturnsCorrectCommandInfo()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "command"]),
            CommandType = typeof(DummyCommand)
        };

        var cmdInfo2 = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "other"]),
            CommandType = typeof(DummyCommand2)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo, cmdInfo2]);

        // Act
        var result = store.FindCommandForArgs(["run", "command", "extra", "args"]);

        // Assert
        result
            .Should()
            .BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandForArgs_WithOnePartCommandAndExtraArgs_ReturnsCommandInfo()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "command", "list"]),
            CommandType = typeof(DummyCommand)
        };

        var cmdInfo2 = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "command", "do"]),
            CommandType = typeof(DummyCommand2)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo, cmdInfo2]);

        // Act
        var result = store.FindCommandForArgs(["run", "command", "do", "extra", "args"]);

        // Assert
        result
            .Should()
            .BeSameAs(cmdInfo2);
    }

    [Fact]
    public void FindCommandForArgs_WithEmptyArgs_ReturnsNull()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandForArgs([]);

        // Assert
        result
            .Should()
            .BeNull();
    }

    [Fact]
    public void AddCommands_WithAlternativeCommands_AddsAliases()
    {
        // Arrange
        var attribute = new CliCommandAttribute(["run"])
            { AlternativeCommands = [["start"]] };

        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = attribute,
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();

        // Act
        store.AddCommands([cmdInfo]);

        // Assert
        var rootNames = store.TreeRootNodes.Select(x => x.Name).ToArray();

        rootNames
            .Should()
            .Contain("run");

        rootNames
            .Should()
            .Contain("start");

        store.FindCommandForArgs(["run"])
            .Should()
            .BeSameAs(cmdInfo);

        store.FindCommandForArgs(["start"])
            .Should()
            .BeSameAs(cmdInfo);
    }

    [Fact]
    public void AddCommands_WithEmptyCommandPath_Throws()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(Array.Empty<string>()),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();

        // Act
        var act = () => store.AddCommands([cmdInfo]);

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }

    [UsedImplicitly]
    private sealed class DummyCommand { }

    [UsedImplicitly]
    private sealed class DummyCommand2 { }
}
