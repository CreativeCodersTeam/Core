using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Commands.Validation;
using CreativeCoders.Cli.Hosting.Exceptions;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Cli.Tests.Hosting.Commands;

public class CliCommandStructureValidatorTests
{
    /// <summary>
    /// Validates that no exception is thrown when command groups and commands are unique.
    /// </summary>
    [Fact]
    public void Validate_WithUniqueGroupsAndCommands_DoesNotThrow()
    {
        // Arrange
        var commandStore = A.Fake<ICliCommandStore>();

        var groupAttributes = new[]
        {
            new CliCommandGroupAttribute(["group", "one"], "Group one"),
            new CliCommandGroupAttribute(["group", "two"], "Group two")
        };

        var commands = new[]
        {
            CreateCommandInfo(["cmd", "one"]),
            CreateCommandInfo(["cmd", "two"])
        };

        A.CallTo(() => commandStore.GroupAttributes)
            .Returns(groupAttributes);

        A.CallTo(() => commandStore.Commands)
            .Returns(commands);

        var validator = new CliCommandStructureValidator();

        // Act
        var action = () => validator.Validate(commandStore);

        // Assert
        action
            .Should()
            .NotThrow();
    }

    /// <summary>
    /// Ensures duplicate group attributes trigger a CliCommandGroupDuplicateException.
    /// </summary>
    [Fact]
    public void Validate_WithDuplicateGroupAttributes_ThrowsException()
    {
        // Arrange
        var commandStore = A.Fake<ICliCommandStore>();

        var duplicateGroup = new CliCommandGroupAttribute(["root", "shared"], "Duplicate group");

        var groupAttributes = new[]
        {
            duplicateGroup,
            duplicateGroup
        };

        var commands = new[]
        {
            CreateCommandInfo(["root", "shared", "first"]),
            CreateCommandInfo(["root", "shared", "second"])
        };

        A.CallTo(() => commandStore.GroupAttributes)
            .Returns(groupAttributes);

        A.CallTo(() => commandStore.Commands)
            .Returns(commands);

        var validator = new CliCommandStructureValidator();

        // Act
        var action = () => validator.Validate(commandStore);

        // Assert
        action
            .Should()
            .ThrowExactly<CliCommandGroupDuplicateException>();
    }

    /// <summary>
    /// Ensures duplicate command paths trigger an AmbiguousCliCommandsException.
    /// </summary>
    [Fact]
    public void Validate_WithDuplicateCommandPaths_ThrowsException()
    {
        // Arrange
        var commandStore = A.Fake<ICliCommandStore>();

        A.CallTo(() => commandStore.GroupAttributes)
            .Returns(Array.Empty<CliCommandGroupAttribute>());

        string[] duplicatePath = ["group", "execute"];

        var commands = new[]
        {
            CreateCommandInfo(duplicatePath),
            CreateCommandInfo(duplicatePath)
        };

        A.CallTo(() => commandStore.Commands)
            .Returns(commands);

        var validator = new CliCommandStructureValidator();

        // Act
        var action = () => validator.Validate(commandStore);

        // Assert
        action
            .Should()
            .ThrowExactly<AmbiguousCliCommandsException>();
    }

    /// <summary>
    /// Ensures duplicate alternative command paths are treated as ambiguous.
    /// </summary>
    [Fact]
    public void Validate_WithDuplicateAlternativeCommandPaths_ThrowsException()
    {
        // Arrange
        var commandStore = A.Fake<ICliCommandStore>();

        A.CallTo(() => commandStore.GroupAttributes)
            .Returns(Array.Empty<CliCommandGroupAttribute>());

        var commands = new[]
        {
            CreateCommandInfo(["main", "command"], ["alias", "run"]),
            CreateCommandInfo(["other", "command"], ["alias", "run"])
        };

        A.CallTo(() => commandStore.Commands)
            .Returns(commands);

        var validator = new CliCommandStructureValidator();

        // Act
        var action = () => validator.Validate(commandStore);

        // Assert
        action
            .Should()
            .ThrowExactly<AmbiguousCliCommandsException>();
    }

    private static CliCommandInfo CreateCommandInfo(string[] commands, string[]? alternativeCommands = null)
    {
        // Builds a command info instance mimicking CLI command definitions for validator inputs.
        var commandAttribute = new CliCommandAttribute(commands)
        {
            AlternativeCommands = alternativeCommands ?? []
        };

        return new CliCommandInfo
        {
            CommandAttribute = commandAttribute,
            CommandType = typeof(object)
        };
    }
}
