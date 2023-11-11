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

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Runtime.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task ExecuteAsync_MiddlewareThrowsException_ReturnCodeIsExceptionReturnCode()
    {
        const int expectedReturnCode = -1357;

        var route = new CliActionRoute(
            typeof(DemoCliController),
            typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoAsync))
            ?? throw new InvalidOperationException(), new[] {"test"});

        var args = new[] {"test"};

        var router = A.Fake<ICliActionRouter>();

        A.CallTo(() => router.FindRoute(A<string[]>.Ignored)).Returns(route);

        var executor = A.Fake<ICliActionExecutor>();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(_ => router)
            .BuildServiceProvider();

        var builder = new CliActionRuntimeBuilder(router, new RoutesBuilder(),
            serviceProvider, executor) as ICliActionRuntimeBuilder;

        var runtime = builder
            .UseExceptionHandling(expectedReturnCode)
            .UseMiddleware<TestErrorMiddleware>()
            .Build();

        // Act
        var result = await runtime.ExecuteAsync(args).ConfigureAwait(false);

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_MiddlewareThrowsException_ExceptionInContextIsSet()
    {
        const int expectedReturnCode = -1357;

        var route = new CliActionRoute(
            typeof(DemoCliController),
            typeof(DemoCliController).GetMethod(nameof(DemoCliController.DoAsync))
            ?? throw new InvalidOperationException(), new[] {"test"});

        var args = new[] {"test"};

        Exception? exception = null;

        var router = A.Fake<ICliActionRouter>();

        A.CallTo(() => router.FindRoute(A<string[]>.Ignored)).Returns(route);

        var executor = A.Fake<ICliActionExecutor>();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(_ => router)
            .BuildServiceProvider();

        var builder = new CliActionRuntimeBuilder(router, new RoutesBuilder(),
            serviceProvider, executor) as ICliActionRuntimeBuilder;

        var runtime = builder
            .UseExceptionHandling(ErrorHandler, expectedReturnCode)
            .UseMiddleware<TestErrorMiddleware>()
            .Build();

        // Act
        var result = await runtime.ExecuteAsync(args).ConfigureAwait(false);

        // Assert
        result
            .Should()
            .Be(expectedReturnCode);

        exception
            .Should()
            .BeOfType<ApplicationException>();

        exception!.Message
            .Should()
            .Be(TestErrorMiddleware.ErrorMessage);
        return;

        void ErrorHandler(CliActionContext x)
        {
            exception = x.Exception;
        }
    }
}

[UsedImplicitly]
public class TestErrorMiddleware : CliActionMiddlewareBase
{
    public const string ErrorMessage = "Error message";

    public TestErrorMiddleware(Func<CliActionContext, Task> next) : base(next) { }

    public override Task InvokeAsync(CliActionContext context)
    {
        throw new ApplicationException(ErrorMessage);
    }
}
