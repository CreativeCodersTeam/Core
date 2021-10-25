using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.CliArguments.Building;
using CreativeCoders.SysConsole.CliArguments.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests
{
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
                        Task.FromResult(new CliCommandResult {ReturnCode = expectedReturnCode});
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
        public async Task ExecuteAsync_CommandGroup_ReturnsGroupCommandResult()
        {
            const int expectedReturnCode = 4321;

            var args = new[] {"group", "command", "param1"};

            var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

            builder
                .AddCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(x =>
                {
                    x.Name = "command";
                    x.OnExecuteAsync = _ => Task.FromResult(new CliCommandResult {ReturnCode = 1234});
                })
                .AddCommandGroup(new CliCommandGroup
                {
                    Name = "group",
                    Commands = new[]
                    {
                        new DelegateCliCommand<TestCommandOptions>
                        {
                            Name = "command",
                            OnExecuteAsync = _ => Task.FromResult(new CliCommandResult
                                {ReturnCode = expectedReturnCode})
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
        public async Task ExecuteAsync_DefaultCommand_ReturnsDefaultCommandResult()
        {
            const int expectedReturnCode = 3456;

            var args = new[] {"default", "command", "param1"};

            var builder = new DefaultCliBuilder(new ServiceCollection().BuildServiceProvider());

            builder
                .AddCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(x =>
                {
                    x.Name = "command";
                    x.OnExecuteAsync = _ => Task.FromResult(new CliCommandResult {ReturnCode = 1234});
                })
                .AddCommandGroup(new CliCommandGroup
                {
                    Name = "group",
                    Commands = new[]
                    {
                        new DelegateCliCommand<TestCommandOptions>
                        {
                            Name = "command",
                            OnExecuteAsync = _ => Task.FromResult(new CliCommandResult {ReturnCode = 4321})
                        }
                    }
                })
                .AddDefaultCommand<DelegateCliCommand<TestCommandOptions>, TestCommandOptions>(x =>
                {
                    x.Name = "";
                    x.OnExecuteAsync = _ =>
                        Task.FromResult(new CliCommandResult {ReturnCode = expectedReturnCode});
                });

            var executor = builder.BuildExecutor();

            // Act
            var result = await executor.ExecuteAsync(args);

            // Assert
            result
                .Should()
                .Be(expectedReturnCode);
        }
    }
}
