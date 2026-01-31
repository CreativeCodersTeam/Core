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
            .Should()
            .HaveCount(1)
            .And
            .Contain(cmdInfo);

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
    public void AddCommands_WithMultipleRootCommands_BuildsSeparateRootNodes()
    {
        // Arrange
        var firstCommand = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run"]),
            CommandType = typeof(DummyCommand)
        };

        var secondCommand = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["stop"]),
            CommandType = typeof(DummyCommand2)
        };

        var store = new CliCommandStore();

        // Act
        store.AddCommands([firstCommand, secondCommand]);

        // Assert
        var rootNodes = store.TreeRootNodes.ToArray();

        rootNodes
            .Should().HaveCount(2);

        rootNodes.Select(x => x.Name)
            .Should().BeEquivalentTo("run", "stop");

        store.FindCommandNode(["run"])?.Node?.CommandInfo
            .Should().BeSameAs(firstCommand);

        store.FindCommandNode(["stop"])?.Node?.CommandInfo
            .Should().BeSameAs(secondCommand);
    }

    [Fact]
    public void AddCommands_WithNestedGroups_BuildsFullTreeHierarchy()
    {
        // Arrange
        var deepCommand = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "group", "do", "now"]),
            CommandType = typeof(DummyCommand)
        };

        var siblingCommand = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["run", "group", "list"]),
            CommandType = typeof(DummyCommand2)
        };

        var store = new CliCommandStore();

        // Act
        store.AddCommands([deepCommand, siblingCommand]);

        // Assert
        var rootNode = store.TreeRootNodes.Single()
            .Should().BeOfType<CliCommandGroupNode>().Which;

        rootNode.Name
            .Should().Be("run");

        var groupNode = rootNode.ChildNodes.Single()
            .Should().BeOfType<CliCommandGroupNode>().Which;

        groupNode.Name
            .Should().Be("group");

        groupNode.ChildNodes
            .Should().HaveCount(2);

        var doGroupNode = groupNode.ChildNodes
            .OfType<CliCommandGroupNode>()
            .Single();

        doGroupNode.Name
            .Should().Be("do");

        var nowCommandNode = doGroupNode.ChildNodes.Single()
            .Should().BeOfType<CliCommandNode>().Which;

        nowCommandNode.Name
            .Should().Be("now");

        nowCommandNode.CommandInfo
            .Should().BeSameAs(deepCommand);

        var listCommandNode = groupNode.ChildNodes
            .OfType<CliCommandNode>()
            .Single();

        listCommandNode.Name
            .Should().Be("list");

        listCommandNode.CommandInfo
            .Should().BeSameAs(siblingCommand);
    }

    [Fact]
    public void AddCommands_WithAlternativeCommands_AndNestedGroups_BuildsAliasesOnTree()
    {
        // Arrange
        var attribute = new CliCommandAttribute(["run", "group", "do"])
        {
            AlternativeCommands = ["execute", "group", "do"]
        };

        var cmdInfo = new CliCommandInfo
        {
            CommandAttribute = attribute,
            CommandType = typeof(DummyCommand)
        };

        var store = new CliCommandStore();

        // Act
        store.AddCommands([cmdInfo]);

        // Assert
        store.TreeRootNodes.Select(x => x.Name)
            .Should().BeEquivalentTo("run", "execute");

        var runRoot = store.TreeRootNodes.First(x => x.Name == "run")
            .Should().BeOfType<CliCommandGroupNode>().Which;

        var executeRoot = store.TreeRootNodes.First(x => x.Name == "execute")
            .Should().BeOfType<CliCommandGroupNode>().Which;

        var runGroupNode = runRoot.ChildNodes.Single();
        runGroupNode.Name
            .Should().Be("group");

        var executeGroupNode = executeRoot.ChildNodes.Single();
        executeGroupNode.Name
            .Should().Be("group");

        runGroupNode.ChildNodes.Single().Name
            .Should().Be("do");

        executeGroupNode.ChildNodes.Single().Name
            .Should().Be("do");

        store.FindCommandNode(["run", "group", "do"])?.Node?.CommandInfo
            .Should().BeSameAs(cmdInfo);

        store.FindCommandNode(["execute", "group", "do"])?.Node?.CommandInfo
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
    public void AddCommands_WithGroupAttribute_NoDuplicatedGroups()
    {
        // Arrange
        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["tools", "admin"]),
            CommandType = typeof(DummyCommand)
        };

        var firstGroupAttribute = new CliCommandGroupAttribute(["tools"], "Tools root group");

        var groupAttributes = new[]
        {
            firstGroupAttribute
        };

        var store = new CliCommandStore();

        // Act
        store.AddCommands([commandInfo], groupAttributes);

        // Assert
        var toolsGroupNode = store.TreeRootNodes
            .Should()
            .ContainSingle(node => node.Name == "tools")
            .Which
            .Should()
            .BeOfType<CliCommandGroupNode>()
            .Which;

        toolsGroupNode.GroupAttribute
            .Should()
            .BeSameAs(firstGroupAttribute);

        toolsGroupNode.ChildNodes
            .OfType<CliCommandNode>()
            .Should()
            .ContainSingle(node => node.Name == "admin");

        store.GroupAttributes
            .Should()
            .BeEquivalentTo(groupAttributes);
    }

    [Fact]
    public void AddCommands_WithGroupAttributes_AssignsAttributesToGroupNodes()
    {
        // Arrange
        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["tools", "admin", "reset"]),
            CommandType = typeof(DummyCommand)
        };

        var firstGroupAttribute = new CliCommandGroupAttribute(["tools"], "Tools root group");
        var secondGroupAttribute = new CliCommandGroupAttribute(["tools", "admin"], "Admin commands");

        var groupAttributes = new[]
        {
            firstGroupAttribute,
            secondGroupAttribute
        };

        var store = new CliCommandStore();

        // Act
        store.AddCommands([commandInfo], groupAttributes);

        // Assert
        var toolsGroupNode = store.TreeRootNodes
            .Should()
            .ContainSingle(node => node.Name == "tools")
            .Which
            .Should()
            .BeOfType<CliCommandGroupNode>()
            .Which;

        toolsGroupNode.GroupAttribute
            .Should()
            .BeSameAs(firstGroupAttribute);

        var adminGroupNode = toolsGroupNode.ChildNodes
            .Should()
            .ContainSingle(node => node.Name == "admin")
            .Which
            .Should()
            .BeOfType<CliCommandGroupNode>()
            .Which;

        adminGroupNode.GroupAttribute
            .Should()
            .BeSameAs(secondGroupAttribute);

        adminGroupNode.ChildNodes
            .OfType<CliCommandNode>()
            .Should()
            .ContainSingle(node => node.Name == "reset");

        store.GroupAttributes
            .Should()
            .BeEquivalentTo(groupAttributes);
    }

    [Fact]
    public void AddCommands_WithGroupAttributes_WithoutMatchingPath_DoesNotAssignAttributes()
    {
        // Arrange
        var commandInfo = new CliCommandInfo
        {
            CommandAttribute = new CliCommandAttribute(["tools", "admin", "reset"]),
            CommandType = typeof(DummyCommand)
        };

        var groupAttributes = new[]
        {
            new CliCommandGroupAttribute(["other"], "Other group"),
            new CliCommandGroupAttribute(["tools", "other"], "Nested other group")
        };

        var store = new CliCommandStore();

        // Act
        store.AddCommands([commandInfo], groupAttributes);

        // Assert
        var toolsGroupNode = store.TreeRootNodes
            .Should()
            .ContainSingle(node => node.Name == "tools")
            .Which
            .Should()
            .BeOfType<CliCommandGroupNode>()
            .Which;

        toolsGroupNode.GroupAttribute
            .Should()
            .BeNull();

        var adminGroupNode = toolsGroupNode.ChildNodes
            .Should()
            .ContainSingle(node => node.Name == "admin")
            .Which
            .Should()
            .BeOfType<CliCommandGroupNode>()
            .Which;

        adminGroupNode.GroupAttribute
            .Should()
            .BeNull();
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
