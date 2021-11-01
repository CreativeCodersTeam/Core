using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;
using CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests
{
    public class ConsoleAppBuilderTests
    {
        [Fact]
        public async Task RunAsync_ConsoleAppControllerRun_ReturnsCorrectReturnCode()
        {
            var args = new[] {"test", "run"};

            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions<Startup>()
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(ConsoleAppTestController.RunReturnCode);
        }

        [Fact]
        public async Task RunAsync_ConsoleAppControllerDo_ReturnsCorrectReturnCode()
        {
            var args = new[] { "test", "do" };

            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions<Startup>()
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(ConsoleAppTestController.DoReturnCode);
        }

        [Fact]
        public async Task RunAsync_ConsoleAppControllerAmbiguousAction_ThrowsException()
        {
            var args = new[] { "test", "do_this" };

            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions<Startup>()
                .Build();

            // Act
            Func<Task> act = async () => await consoleApp.ReThrowExceptions(true).RunAsync();

            // Assert
            await act
                .Should()
                .ThrowAsync<AmbiguousRouteException>();
        }
    }
}
