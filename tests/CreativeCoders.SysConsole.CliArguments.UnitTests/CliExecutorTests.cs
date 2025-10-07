using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Building;
using CreativeCoders.SysConsole.CliArguments.Commands;
using CreativeCoders.SysConsole.CliArguments.UnitTests.TestData;
using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests;

public class CliExecutorTests
{
    [Fact]
    public async Task ExecuteAsync_Command_ReturnsCommandResult()
    {
        const int expectedReturnCode = 1234;

        var args = new[] {"command", "param1"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        builder
            .AddCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(x =>
            {
                x.Name = "command";
                x.OnExecuteAsync = _ =>
                    Task.FromResult(new CliCommandResult(expectedReturnCode));
            });

        var executor = builder.BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_CommandClass_ReturnsCommandResult()
    {
        var args = new[] {"command", "param1"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        builder
            .AddCommand<TestCommand>();

        var executor = builder.BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(TestCommand.ReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_CommandGroup_ReturnsGroupCommandResult()
    {
        const int expectedReturnCode = 4321;

        var args = new[] {"group", "command", "param1"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        builder
            .AddCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(x =>
            {
                x.Name = "command";
                x.OnExecuteAsync = _ => Task.FromResult(new CliCommandResult(1234));
            })
            .AddCommandGroup(new CliCommandGroup
            {
                Name = "group",
                Commands = new[]
                {
                    new DelegateCliCommand<TestCommandOptions>
                    {
                        Name = "command",
                        OnExecuteAsync = _ => Task.FromResult(new CliCommandResult(expectedReturnCode))
                    }
                }
            });

        var executor = builder.BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_CommandGroupWithBuilder_ReturnsGroupCommandResult()
    {
        const int expectedReturnCode = 4321;

        var args = new[] {"group", "command", "param1"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        builder
            .AddCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(x =>
            {
                x.Name = "command";
                x.OnExecuteAsync = _ => Task.FromResult(new CliCommandResult(1234));
            })
            .AddCommandGroup(x => x
                .SetName("group")
                .AddCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(cmd =>
                {
                    cmd.Name = "command";
                    cmd.OnExecuteAsync = _ =>
                        Task.FromResult(new CliCommandResult(expectedReturnCode));
                }));

        var executor = builder.BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_DefaultCommand_ReturnsDefaultCommandResult()
    {
        const int expectedReturnCode = 3456;

        var args = new[] {"default", "command", "param1"};

        var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

        builder
            .AddCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(x =>
            {
                x.Name = "command";
                x.OnExecuteAsync = _ => Task.FromResult(new CliCommandResult(1234));
            })
            .AddCommandGroup(new CliCommandGroup
            {
                Name = "group",
                Commands = new[]
                {
                    new DelegateCliCommand<TestCommandOptions>
                    {
                        Name = "command",
                        OnExecuteAsync = _ => Task.FromResult(new CliCommandResult(4321))
                    }
                }
            })
            .AddDefaultCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(x =>
            {
                x.Name = "";
                x.OnExecuteAsync = _ =>
                    Task.FromResult(new CliCommandResult(expectedReturnCode));
            });

        var executor = builder.BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_NoCommands_DefaultErrorReturnCodeIsReturned()
    {
        const int expectedReturnCode = -12345;

        var executor = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider())
            .SetDefaultErrorReturnCode(expectedReturnCode)
            .BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(Array.Empty<string>());

        // Assert
        executor.DefaultErrorReturnCode
            .Should()
            .Be(expectedReturnCode);

        result
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_NoGroupCommandArgGiven_DefaultCommandInGroupIsExecuted()
    {
        var args = new[] {"group"};

        var executor = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider())
            .AddCommandGroup(x => x
                .SetName("group")
                .AddCommand<TestDefaultCommand>())
            .BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(TestDefaultCommand.ReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_NoCommandArgGiven_DefaultCommandIsExecuted()
    {
        var args = new[] {"group"};

        var executor = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider())
            .AddCommand<TestDefaultCommand>()
            .BuildExecutor();

        // Act
        var result = await executor.ExecuteAsync(args);

        // Assert
        result
            .Should()
            .Be(TestDefaultCommand.ReturnCode);
    }
}
