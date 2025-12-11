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
    public void FindCommandGroupNode_WithExactExistingGroupArgs_ReturnsNull()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "group", "do"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandGroupNode(["run", "group"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.Name
            .Should().Be("group");

        result.RemainingArgs
            .Should().BeEmpty();
    }

    [Fact]
    public void FindCommandGroupNode_WithCommandToken_ReturnsNullParent()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "group", "do"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandGroupNode(["run", "group", "do"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.Name
            .Should().Be("group");

        result.RemainingArgs
            .Should().BeEquivalentTo("do");
    }

    [Fact]
    public void FindCommandGroupNode_WithUnknownTokenAfterExistingGroup_ReturnsLastExistingGroup()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "group", "do"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandGroupNode(["run", "group", "unknown"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.Name
            .Should().Be("group");

        result.RemainingArgs
            .Should().BeEquivalentTo("unknown");
    }

    [Fact]
    public void FindCommandGroupNode_WithUnknownRootToken_ReturnsNull()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "group", "do"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandGroupNode(["unknown"]);

        // Assert
        result
            .Should().BeNull();
    }

    [Fact]
    public void FindCommandGroupNode_WithEmptyArgs_ReturnsNull()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "group", "do"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandGroupNode([]);

        // Assert
        result
            .Should().BeNull();
    }

    [Fact]
    public void FindCommandGroupNode_WithCommandTokenAndExtraArgs_ReturnsNullParent()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "group", "do"]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();
        store.AddCommands([cmdInfo]);

        // Act
        var result = store.FindCommandGroupNode(["run", "group", "do", "extra", "args"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.Name
            .Should().Be("group");

        result.RemainingArgs
            .Should().BeEquivalentTo("do", "extra", "args");
    }

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
            .Should().Contain(cmdInfo);

        var rootNodes = store.TreeRootNodes.ToArray();

        rootNodes
            .Should().HaveCount(1);

        var node = rootNodes[0]
            .Should().BeOfType<CliCommandNode>().Which;
        node.Name
            .Should().Be("run");

        node.CommandInfo
            .Should().BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandNode_WithSingleCommand_ReturnsCommandInfo()
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
        var result = store.FindCommandNode(["run"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.CommandInfo
            .Should().BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandNode_WithMultiPartCommand_ReturnsCommandInfo()
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
        var result = store.FindCommandNode(["run", "the", "command"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.CommandInfo
            .Should().BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandNode_WithMultiPartCommandAndExtraArgs_ReturnsCommandInfo()
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
        var result = store.FindCommandNode(["run", "the", "command", "extra", "args"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.CommandInfo
            .Should().BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandNode_MultipleCommandsInSameGroup_ReturnsCorrectCommandInfo()
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
        var result = store.FindCommandNode(["run", "command", "extra", "args"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.CommandInfo
            .Should().BeSameAs(cmdInfo);
    }

    [Fact]
    public void FindCommandNode_WithOnePartCommandAndExtraArgs_ReturnsCommandInfo()
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
        var result = store.FindCommandNode(["run", "command", "do", "extra", "args"]);

        // Assert
        result
            .Should().NotBeNull();

        result.Node
            .Should().NotBeNull();

        result.Node.CommandInfo
            .Should().BeSameAs(cmdInfo2);
    }

    [Fact]
    public void FindCommandNode_WithEmptyArgs_ReturnsNull()
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
        var result = store.FindCommandNode([]);

        // Assert
        result
            .Should().BeNull();
    }

    [Fact]
    public void AddCommands_WithAlternativeCommands_AddsAliases()
    {
        // Arrange
        var attribute = new CliCommandAttribute(["run"])
            { AlternativeCommands = ["start"] };

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

        store.FindCommandNode(["run"])?.Node?.CommandInfo
            .Should()
            .BeSameAs(cmdInfo);

        store.FindCommandNode(["start"])?.Node?.CommandInfo
            .Should()
            .BeSameAs(cmdInfo);
    }

    [Fact]
    public void AddCommands_WithEmptyCommandPath_Throws()
    {
        // Arrange
        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute([]),
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();

        // Act
        var act = () => store.AddCommands([cmdInfo]);

        // Assert
        act
            .Should().Throw<InvalidOperationException>();
    }

    [UsedImplicitly]
    private sealed class DummyCommand { }

    [UsedImplicitly]
    private sealed class DummyCommand2 { }
}
