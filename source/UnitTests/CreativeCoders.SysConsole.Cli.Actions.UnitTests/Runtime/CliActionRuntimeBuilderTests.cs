using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;
using CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;
using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Runtime
{
    public class CliActionRuntimeBuilderTests
    {
        [Fact]
        public async Task ExecuteAsync_MiddlewareIsRegistered_MiddlewareIsCalled()
        {
            var route = new CliActionRoute(typeof(DemoCliController),
                typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoAsync)), new[] {"test"});

            var args = new[] {"test"};

            var router = A.Fake<ICliActionRouter>();

            A.CallTo(() => router.FindRoute(A<string[]>.Ignored)).Returns(route);

            var builder = new CliActionRuntimeBuilder(router, new RoutesBuilder(),
                new ServiceCollection().AddSingleton<ICliActionRouter>(_ => router).BuildServiceProvider());

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

        //[Fact]
        public async Task ExecuteAsync_StringMiddlewareIsRegistered_MiddlewareIsCalled()
        {
            var route = new CliActionRoute(typeof(DemoCliController),
                typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoAsync)), new[] { "test" });

            var args = new[] { "test" };

            var router = A.Fake<ICliActionRouter>();

            A.CallTo(() => router.FindRoute(A<string[]>.Ignored)).Returns(route);

            var builder = new CliActionRuntimeBuilder(router, new RoutesBuilder(),
                new ServiceCollection().AddSingleton<ICliActionRouter>(_ => router).BuildServiceProvider());

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

    public class StringTestMiddleware : CliActionMiddlewareBase
    {
        private readonly string _text;

        public StringTestMiddleware(Func<CliActionContext, Task> next, string text) : base(next)
        {
            _text = text;
        }

        public override Task InvokeAsync(CliActionContext context)
        {
            return Task.FromResult(new CliActionResult(_text?.GetHashCode() ?? 0));
        }
    }

    [UsedImplicitly]
    public class FirstTestMiddleware : CliActionMiddlewareBase
    {
        public const int ReturnCode = 1357;

        public FirstTestMiddleware(Func<CliActionContext, Task> next) : base(next) { }

        public override async Task InvokeAsync(CliActionContext context)
        {
            context.ReturnCode = ReturnCode;

            IsCalled = true;

            await Next(context);
        }

        public static bool IsCalled { get; private set; }
    }

    [UsedImplicitly]
    public class SecondTestMiddleware : CliActionMiddlewareBase
    {
        public const int ReturnCode = 2345;

        public SecondTestMiddleware(Func<CliActionContext, Task> next) : base(next) { }

        public override async Task InvokeAsync(CliActionContext context)
        {
            if (context.ReturnCode == FirstTestMiddleware.ReturnCode)
            {
                context.ReturnCode = ReturnCode;
            }

            IsCalled = true;

            await Next(context);
        }

        public static bool IsCalled { get; private set; }
    }
}
