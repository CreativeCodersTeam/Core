using System.Reflection;
using AwesomeAssertions;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Commands.Validation;
using CreativeCoders.Cli.Hosting.Help;
using FakeItEasy;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Cli.Tests.Hosting;

public class DefaultCliHostBuilderTests
{
    [Fact]
    public void Build_WithHelpEnabled_RegistersHelpHandlerAndSettings()
    {
        // Arrange
        var builder = CreateBuilder();

        builder.SkipScanEntryAssembly();
        builder.EnableHelp(HelpCommandKind.Command);

        var commandScanner = A.Fake<IAssemblyCommandScanner>();
        var commandStore = A.Fake<ICliCommandStore>();
        var validator = A.Fake<ICliCommandStructureValidator>();
        var cliHost = A.Fake<ICliHost>();

        A.CallTo(() => commandScanner.ScanForCommands(A<Assembly[]>.Ignored))
            .Returns(CreateScanResult());

        SubstituteServices(builder, commandScanner, commandStore, validator, cliHost);

        var services = GetBuiltServiceProvider(builder);

        // Act
        var host = builder.Build();

        // Assert
        host
            .Should()
            .Be(cliHost);

        services
            .GetRequiredService<ICliCommandHelpHandler>()
            .Should()
            .BeOfType<CliCommandHelpHandler>();

        services
            .GetRequiredService<HelpHandlerSettings>()
            .CommandKind
            .Should()
            .Be(HelpCommandKind.Command);
    }

    [Fact]
    public void Build_WithHelpDisabled_RegistersDisabledHelpHandler()
    {
        // Arrange
        var builder = CreateBuilder();

        builder.SkipScanEntryAssembly();

        var commandScanner = A.Fake<IAssemblyCommandScanner>();
        var commandStore = A.Fake<ICliCommandStore>();
        var validator = A.Fake<ICliCommandStructureValidator>();
        var cliHost = A.Fake<ICliHost>();

        A.CallTo(() => commandScanner.ScanForCommands(A<Assembly[]>.Ignored))
            .Returns(CreateScanResult());

        SubstituteServices(builder, commandScanner, commandStore, validator, cliHost);

        var services = GetBuiltServiceProvider(builder);

        // Act
        var host = builder.Build();

        // Assert
        host
            .Should()
            .Be(cliHost);

        services
            .GetRequiredService<ICliCommandHelpHandler>()
            .Should()
            .BeOfType<DisabledCommandHelpHandler>();
    }

    [Fact]
    public void Build_CallsValidatorWithCommandStore()
    {
        // Arrange
        var builder = CreateBuilder();

        builder.SkipScanEntryAssembly();

        var commandScanner = A.Fake<IAssemblyCommandScanner>();
        var commandStore = A.Fake<ICliCommandStore>();
        var validator = A.Fake<ICliCommandStructureValidator>();
        var cliHost = A.Fake<ICliHost>();

        var commandInfos = new[]
        {
            new CliCommandInfo
            {
                CommandType = typeof(DefaultCliHostBuilderTests),
                CommandAttribute = new CliCommandAttribute(["test"])
            }
        };

        A.CallTo(() => commandScanner.ScanForCommands(A<Assembly[]>.Ignored))
            .Returns(new AssemblyScanResult
            {
                CommandInfos = commandInfos,
                GroupAttributes = []
            });

        SubstituteServices(builder, commandScanner, commandStore, validator, cliHost);

        // Act
        builder.Build();

        // Assert
        A.CallTo(() => validator.Validate(commandStore))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                commandStore.AddCommands(commandInfos, A<IEnumerable<CliCommandGroupAttribute>>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ScanAssemblies_AddsAssembliesToScanList()
    {
        // Arrange
        var builder = CreateBuilder();

        var assemblies = new[] { Assembly.GetExecutingAssembly(), typeof(DefaultCliHostBuilder).Assembly };

        builder.SkipScanEntryAssembly();

        var commandScanner = A.Fake<IAssemblyCommandScanner>();
        var commandStore = A.Fake<ICliCommandStore>();
        var validator = A.Fake<ICliCommandStructureValidator>();
        var cliHost = A.Fake<ICliHost>();

        A.CallTo(() =>
                commandScanner.ScanForCommands(
                    A<Assembly[]>.That.IsSameSequenceAs(assemblies.AsEnumerable())))
            .Returns(new AssemblyScanResult
            {
                CommandInfos = [],
                GroupAttributes = []
            });

        SubstituteServices(builder, commandScanner, commandStore, validator, cliHost);

        // Act
        builder.ScanAssemblies(assemblies).Build();

        // Assert
        A.CallTo(() =>
                commandScanner.ScanForCommands(
                    A<Assembly[]>.That.IsSameSequenceAs(assemblies.AsEnumerable())))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void SkipScanEntryAssembly_True_DoesNotScanEntryAssembly()
    {
        // Arrange
        var builder = CreateBuilder();

        builder.SkipScanEntryAssembly();

        var commandScanner = A.Fake<IAssemblyCommandScanner>();
        var commandStore = A.Fake<ICliCommandStore>();
        var validator = A.Fake<ICliCommandStructureValidator>();
        var cliHost = A.Fake<ICliHost>();

        A.CallTo(() => commandScanner.ScanForCommands(A<Assembly[]>.Ignored))
            .Returns(new AssemblyScanResult
            {
                CommandInfos = Array.Empty<CliCommandInfo>(),
                GroupAttributes = Array.Empty<CliCommandGroupAttribute>()
            });

        SubstituteServices(builder, commandScanner, commandStore, validator, cliHost);

        // Act
        builder.Build();

        // Assert
        A.CallTo(() => commandScanner.ScanForCommands(A<Assembly[]>.Ignored))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ConfigureServices_InvokesProvidedAction()
    {
        // Arrange
        var builder = CreateBuilder();

        builder.SkipScanEntryAssembly();

        var wasCalled = false;

        builder.ConfigureServices(_ => wasCalled = true);

        var commandScanner = A.Fake<IAssemblyCommandScanner>();
        var commandStore = A.Fake<ICliCommandStore>();
        var validator = A.Fake<ICliCommandStructureValidator>();
        var cliHost = A.Fake<ICliHost>();

        A.CallTo(() => commandScanner.ScanForCommands(A<Assembly[]>.Ignored))
            .Returns(CreateScanResult());

        SubstituteServices(builder, commandScanner, commandStore, validator, cliHost);

        // Act
        builder.Build();

        // Assert
        wasCalled
            .Should()
            .BeTrue();
    }

    [Fact]
    public void UseContext_AddsContextToServices()
    {
        // Arrange
        var builder = CreateBuilder();

        builder.SkipScanEntryAssembly();

        builder.UseContext<DummyContext>((_, context) => context.Value = 42);

        var commandScanner = A.Fake<IAssemblyCommandScanner>();
        var commandStore = A.Fake<ICliCommandStore>();
        var validator = A.Fake<ICliCommandStructureValidator>();
        var cliHost = A.Fake<ICliHost>();

        A.CallTo(() => commandScanner.ScanForCommands(A<Assembly[]>.Ignored))
            .Returns(CreateScanResult());

        SubstituteServices(builder, commandScanner, commandStore, validator, cliHost);

        var services = GetBuiltServiceProvider(builder);

        // Act
        builder.Build();

        // Assert
        var context = services.GetRequiredService<DummyContext>();

        context
            .Should()
            .NotBeNull();

        context.Value
            .Should()
            .Be(42);
    }

    private static DefaultCliHostBuilder CreateBuilder()
    {
        return new DefaultCliHostBuilder();
    }

    private static void SubstituteServices(
        DefaultCliHostBuilder builder,
        IAssemblyCommandScanner commandScanner,
        ICliCommandStore commandStore,
        ICliCommandStructureValidator validator,
        ICliHost cliHost)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(commandScanner);
            services.AddSingleton(commandStore);
            services.AddSingleton(validator);
            services.AddSingleton(cliHost);
        });
    }

    private static IServiceProvider GetBuiltServiceProvider(DefaultCliHostBuilder builder)
    {
        var method = typeof(DefaultCliHostBuilder)
                         .GetMethod(
                             "BuildServiceProvider", BindingFlags.Instance | BindingFlags.NonPublic)
                     ?? throw new InvalidOperationException();

        return (IServiceProvider?)method.Invoke(builder, []) ?? throw new InvalidOperationException();
    }

    private static AssemblyScanResult CreateScanResult()
    {
        return new AssemblyScanResult
        {
            CommandInfos =
            [
                new CliCommandInfo
                {
                    CommandType = typeof(DefaultCliHostBuilderTests),
                    CommandAttribute = new CliCommandAttribute(["test"])
                }
            ],
            GroupAttributes = []
        };
    }

    [UsedImplicitly]
    private sealed class DummyContext : CliCommandContext
    {
        public int Value { get; set; }
    }
}
