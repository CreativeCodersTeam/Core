using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
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
        var scanResult = scanner.ScanForCommands(assemblies);

        // Assert
        scanResult.CommandInfos
            .Should()
            .BeEmpty();

        scanResult.GroupAttributes
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void Scan_WithCliCommandGroupAttribute_ReturnsGroupAttribute()
    {
        // Arrange
        var assemblyWithGroup = CreateAssemblyWithGroupAttribute();

        var commandInfoCreator = A.Fake<ICommandInfoCreator>();
        var scanner = new AssemblyCommandScanner(commandInfoCreator);

        // Act
        var scanResult = scanner.ScanForCommands([assemblyWithGroup]).GroupAttributes.ToArray();

        // Assert
        scanResult
            .Should()
            .HaveCount(1);

        var groupAttribute = scanResult.Single();

        groupAttribute.Commands
            .Should()
            .BeEquivalentTo("group", "one");

        groupAttribute.Description
            .Should()
            .Be("Test group");
    }

    [CliCommand(["one"])]
    private sealed class DummyCommandOne { }

    [CliCommand(["two", "2"])]
    private sealed class DummyCommandTwo { }

    [UsedImplicitly]
    private sealed class NonCommandType { }

    private static Assembly CreateAssemblyWithGroupAttribute()
    {
        var assemblyName = new AssemblyName("CliCommandGroupAttributeTestAssembly");

        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        assemblyBuilder.DefineDynamicModule("MainModule");

        var groupAttributeConstructor = typeof(CliCommandGroupAttribute)
            .GetConstructor(new[] { typeof(string[]), typeof(string) })!;

        var attributeBuilder = new CustomAttributeBuilder(
            groupAttributeConstructor,
            new object[] { new[] { "group", "one" }, "Test group" });

        assemblyBuilder.SetCustomAttribute(attributeBuilder);

        return assemblyBuilder;
    }
}
