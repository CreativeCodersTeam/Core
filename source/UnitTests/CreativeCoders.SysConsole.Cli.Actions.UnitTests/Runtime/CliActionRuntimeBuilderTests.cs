using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
using CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Runtime
{
    public class CliActionRuntimeBuilderTests
    {
        [Fact]
        public async Task ExecuteAsync_MiddlewareIsRegistered_MiddlewareIsCalled()
        {
            var route = new CliActionRoute(
                typeof(DemoCliController),
                typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoAsync))
                ?? throw new InvalidOperationException(), new[] {"test"});

            var args = new[] {"test"};

            var executor = A.Fake<ICliActionExecutor>();

            var router = A.Fake<ICliActionRouter>();

            A.CallTo(() => router.FindRoute(A<string[]>.Ignored)).Returns(route);

            var serviceProvider = new ServiceCollection()
                .AddSingleton(_ => router)
                .BuildServiceProvider();

            var builder = new CliActionRuntimeBuilder(router, new RoutesBuilder(),
                serviceProvider, executor) as ICliActionRuntimeBuilder;

            var runtime = builder
                .UseMiddleware<FirstTestMiddleware>()
                .UseMiddleware<SecondTestMiddleware>()
                .UseMiddleware<CliRoutingMiddleware>()
                .Build();

            // Act
            var result = await runtime.ExecuteAsync(args);

            // Assert
            FirstTestMiddleware.IsCalled
                .Should()
                .BeTrue();

            SecondTestMiddleware.IsCalled
                .Should()
                .BeTrue();

            result
                .Should()
                .Be(SecondTestMiddleware.ReturnCode);
        }

        [Fact]
        public async Task ExecuteAsync_StringMiddlewareIsRegistered_MiddlewareIsCalled()
        {
            var route = new CliActionRoute(
                typeof(DemoCliController),
                typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoAsync)) ?? throw new InvalidOperationException(),
                new[] { "test" });

            var args = new[] { "test" };

            var router = A.Fake<ICliActionRouter>();

            A.CallTo(() => router.FindRoute(A<string[]>.Ignored)).Returns(route);

            var executor = A.Fake<ICliActionExecutor>();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(_ => router)
                .BuildServiceProvider();

            var builder = new CliActionRuntimeBuilder(router, new RoutesBuilder(),
                serviceProvider, executor) as ICliActionRuntimeBuilder;

            var runtime = builder
                .UseMiddleware<StringTestMiddleware>("TestText")
                .UseMiddleware<CliRoutingMiddleware>()
                .Build();

            // Act
            var result = await runtime.ExecuteAsync(args);

            // Assert
            result
                .Should()
                .Be("TestText".GetHashCode());
        }
    }
}
