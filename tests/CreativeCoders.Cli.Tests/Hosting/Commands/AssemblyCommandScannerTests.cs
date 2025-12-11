using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using JetBrains.Annotations;
using Xunit;
using FakeItEasy;

namespace CreativeCoders.Cli.Tests.Hosting.Commands;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class AssemblyCommandScannerTests
{
    [Fact]
    public void Scan_TypesWithCliCommandAttribute_AreReturned()
    {
        // Arrange
        var assemblies = new[] { typeof(DummyCommandOne).Assembly };

        var commandInfoCreator = A.Fake<ICommandInfoCreator>();

        A.CallTo(() => commandInfoCreator.Create(typeof(DummyCommandOne)))
            .Returns(new CliCommandInfo
            {
                CommandType = typeof(DummyCommandOne),
                CommandAttribute = typeof(DummyCommandOne)
                    .GetCustomAttribute<CliCommandAttribute>()!
            });

        A.CallTo(() => commandInfoCreator.Create(typeof(DummyCommandTwo)))
            .Returns(new CliCommandInfo
            {
                CommandType = typeof(DummyCommandTwo),
                CommandAttribute = typeof(DummyCommandTwo)
                    .GetCustomAttribute<CliCommandAttribute>()!
            });

        var scanner = new AssemblyCommandScanner(commandInfoCreator);

        // Act
        var result = scanner.ScanForCommands(assemblies).CommandInfos.ToArray();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .HaveCount(2);

        var types = result.Select(x => x.CommandType).ToArray();

        types
            .Should()
            .Contain(typeof(DummyCommandOne));

        types
            .Should()
            .Contain(typeof(DummyCommandTwo));

        var cmdOneInfo = result.Single(x => x.CommandType == typeof(DummyCommandOne));

        cmdOneInfo.CommandAttribute.Commands
            .Should()
            .BeEquivalentTo("one");

        var cmdTwoInfo = result.Single(x => x.CommandType == typeof(DummyCommandTwo));

        cmdTwoInfo.CommandAttribute.Commands
            .Should()
            .BeEquivalentTo("two", "2");
    }

    [Fact]
    public void Scan_TypesWithoutCliCommandAttribute_AreSkipped()
    {
        // Arrange
        var assemblies = new[] { typeof(NonCommandType).Assembly };

        var commandInfoCreator = A.Fake<ICommandInfoCreator>();
        var scanner = new AssemblyCommandScanner(commandInfoCreator);

        // Act
        var result = scanner.ScanForCommands(assemblies).CommandInfos.ToArray();

        // Assert
        result
            .Select(x => x.CommandType)
            .Should()
            .NotContain(typeof(NonCommandType));
    }

    [Fact]
    public void Scan_WithEmptyAssemblies_ReturnsEmpty()
    {
        // Arrange
        var assemblies = Array.Empty<Assembly>();

        var commandInfoCreator = A.Fake<ICommandInfoCreator>();
        var scanner = new AssemblyCommandScanner(commandInfoCreator);

        // Act
        var result = scanner.ScanForCommands(assemblies).CommandInfos.ToArray();

        // Assert
        result
            .Should()
            .BeEmpty();
    }

    [CliCommand(["one"])]
    private sealed class DummyCommandOne { }

    [CliCommand(["two", "2"])]
    private sealed class DummyCommandTwo { }

    [UsedImplicitly]
    private sealed class NonCommandType { }
}
