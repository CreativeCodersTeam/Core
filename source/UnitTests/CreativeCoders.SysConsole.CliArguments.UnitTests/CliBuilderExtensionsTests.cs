using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Building;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.UnitTests.TestData;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests;

public class CliBuilderExtensionsTests
{
    [Fact]
    public async Task ExecuteAsync_CommandWithOptions_ReturnsCommandResult()
    {
        const int expectedReturnCode = 1234;

        var args = new[] {"command", "param1"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        builder
            .AddCommand<TestCommandOptions>("command",
                _ => Task.FromResult(new CliCommandResult(expectedReturnCode)));

        var executor = builder.BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_WithOptions_OptionsArePassedToExecute()
    {
        var args = new[] {"command", "-t", "param1"};

        TestCommandOptions? options = null;

        var executor = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider())
            .AddCommand<TestCommandOptions>("command", x =>
            {
                options = x;
                return Task.FromResult(new CliCommandResult(1357));
            })
            .BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(1357);

        options
            .Should()
            .NotBeNull();

        options!.Text
            .Should()
            .Be("param1");
    }

    [Fact]
    public async Task ExecuteAsync_CommandAddedViaModule_ReturnsResultFromCommand()
    {
        var args = new[] {"command"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        var executor = builder
            .AddModule<TestModuleWithOneCommand>()
            .BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(TestModuleWithOneCommand.ReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_CommandAddedViaAssemblyModules_ReturnsResultFromCommand()
    {
        var args = new[] {"command"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        var executor = builder
            .AddModules(typeof(TestModuleWithOneCommand).Assembly)
            .BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(TestModuleWithOneCommand.ReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_InvalidModuleType_ReturnsDefaultErrorCode()
    {
        const int expectedReturnCode = -2468;

        var args = new[] {"command"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        var executor = builder
            .SetDefaultErrorReturnCode(expectedReturnCode)
            .AddModule(typeof(int))
            .BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);
    }
}
