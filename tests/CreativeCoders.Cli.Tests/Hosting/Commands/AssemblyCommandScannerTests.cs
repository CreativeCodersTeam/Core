using System.Reflection;
using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using JetBrains.Annotations;
using Xunit;

namespace CreativeCoders.Cli.Tests.Hosting.Commands;

public class AssemblyCommandScannerTests
{
    [Fact]
    public void Scan_TypesWithCliCommandAttribute_AreReturned()
    {
        // Arrange
        var assemblies = new[] { typeof(DummyCommandOne).Assembly };
        var scanner = new AssemblyCommandScanner(new CommandInfoCreator());

        // Act
        var result = scanner.Scan(assemblies).ToArray();

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
        var scanner = new AssemblyCommandScanner(new CommandInfoCreator());

        // Act
        var result = scanner.Scan(assemblies).ToArray();

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
        var scanner = new AssemblyCommandScanner(new CommandInfoCreator());

        // Act
        var result = scanner.Scan(assemblies).ToArray();

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
