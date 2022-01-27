using System;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
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
            var exception = (await act
                .Should()
                .ThrowAsync<AmbiguousRouteException>())
                .Which;

            exception.Arguments
                .Should()
                .HaveCount(args.Length)
                .And
                .ContainInOrder(args);

            exception.Routes
                .Should()
                .HaveCount(2);

            exception.Routes.First().ActionMethod
                .Should()
                .BeSameAs(typeof(ConsoleAppTestController).GetMethod(nameof(ConsoleAppTestController
                    .DoThis1Async)));

            exception.Routes.Last().ActionMethod
                .Should()
                .BeSameAs(typeof(ConsoleAppTestController).GetMethod(nameof(ConsoleAppTestController
                    .DoThis2Async)));
        }

        [Theory]
        [InlineData("execute")]
        [InlineData("execute", "this")]
        [InlineData("controller")]
        [InlineData("controller", "execute")]
        [InlineData("controller", "execute", "this")]
        [InlineData]
        public async Task RunSync_DifferentRoutesToDefaultAction_ActionIsExecuted(params string[] args)
        {
            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions(x =>
                    x.AddController<TestMultiRouteCliController>().UseMiddleware<CliRoutingMiddleware>())
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(TestMultiRouteCliController.ExecuteReturnCode);
        }

        [Theory]
        [InlineData("execute")]
        [InlineData("execute", "this")]
        [InlineData("controller")]
        [InlineData("controller", "execute")]
        [InlineData("controller", "execute", "this")]
        [InlineData]
        public async Task RunSync_AddControllerWithTypeParamDifferentRoutesToDefaultAction_ActionIsExecuted(params string[] args)
        {
            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions(x =>
                    x.AddController(typeof(TestMultiRouteCliController)).UseMiddleware<CliRoutingMiddleware>())
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(TestMultiRouteCliController.ExecuteReturnCode);
        }

        [Theory]
        [InlineData("execute", "-t", "HelloWorld")]
        [InlineData("execute", "this", "--text", "HelloWorld")]
        [InlineData("controller", "-t", "HelloWorld")]
        [InlineData("controller", "execute", "--text", "HelloWorld")]
        [InlineData("controller", "execute", "this", "-t", "HelloWorld")]
        public async Task RunSync_DifferentRoutesToDefaultActionWithOptions_ActionIsExecutedWithOptions(params string[] args)
        {
            var consoleApp = new ConsoleAppBuilder(args)
                .UseActions(x =>
                    x.AddController<TestMultiRouteCliController>().UseMiddleware<CliRoutingMiddleware>())
                .Build();

            // Act
            var result = await consoleApp.RunAsync();

            // Assert
            result
                .Should()
                .Be(args.Last().GetHashCode());
        }
    }
}
