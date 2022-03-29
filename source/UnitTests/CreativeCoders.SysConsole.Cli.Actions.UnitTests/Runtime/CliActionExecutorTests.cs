using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Runtime;

public class CliActionExecutorTests
{
    [Fact]
    public async Task ExecuteAsync_MethodReturnsNull_ThrowsException()
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var executor = new CliActionExecutor(serviceProvider);

        var context = CreateTestContext(nameof(ActionExecutorTestClass.ExecuteNull));

        // Act
        Func<Task> act = async () => await executor.ExecuteAsync(context);

        // Assert
        await act
            .Should()
            .ThrowAsync<ActionReturnValueNullException>();
    }

    [Fact]
    public async Task ExecuteAsync_MethodReturnsInvalidResult_ThrowsException()
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var executor = new CliActionExecutor(serviceProvider);

        var context = CreateTestContext(nameof(ActionExecutorTestClass.ExecuteInvalidReturnType));

        // Act
        Func<Task> act = async () => await executor.ExecuteAsync(context);

        // Assert
        (await act
                .Should()
                .ThrowAsync<ActionReturnTypeNotSupportedException>())
            .Which.ReturnType
            .Should()
            .Be<string>();
    }

    [Fact]
    public async Task ExecuteAsync_MethodReturnsAsyncActionResult_ReturnCodeIsCorrect()
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var executor = new CliActionExecutor(serviceProvider);

        var context = CreateTestContext(nameof(ActionExecutorTestClass.ExecuteWithActionResultAsync));

        // Act
        await executor.ExecuteAsync(context);

        // Assert
        context.ReturnCode
            .Should()
            .Be(ActionExecutorTestClass.AsyncActionResultReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_MethodReturnsAsyncInt_ReturnCodeIsCorrect()
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var executor = new CliActionExecutor(serviceProvider);

        var context = CreateTestContext(nameof(ActionExecutorTestClass.ExecuteWithIntAsync));

        // Act
        await executor.ExecuteAsync(context);

        // Assert
        context.ReturnCode
            .Should()
            .Be(ActionExecutorTestClass.AsyncIntReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_MethodReturnsAsyncTask_ReturnCodeIsCorrect()
    {
        const int expectedReturnCode = 9876;

        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var executor = new CliActionExecutor(serviceProvider);

        var context = CreateTestContext(nameof(ActionExecutorTestClass.ExecuteAsync));

        context.ReturnCode = expectedReturnCode;

        // Act
        await executor.ExecuteAsync(context);

        // Assert
        context.ReturnCode
            .Should()
            .Be(expectedReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_MethodReturnsInt_ReturnCodeIsCorrect()
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var executor = new CliActionExecutor(serviceProvider);

        var context = CreateTestContext(nameof(ActionExecutorTestClass.ExecuteWithInt));

        // Act
        await executor.ExecuteAsync(context);

        // Assert
        context.ReturnCode
            .Should()
            .Be(ActionExecutorTestClass.IntReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_MethodReturnsActionResult_ReturnCodeIsCorrect()
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var executor = new CliActionExecutor(serviceProvider);

        var context = CreateTestContext(nameof(ActionExecutorTestClass.ExecuteWithActionResult));

        // Act
        await executor.ExecuteAsync(context);

        // Assert
        context.ReturnCode
            .Should()
            .Be(ActionExecutorTestClass.ActionResultReturnCode);
    }

    [Fact]
    public async Task ExecuteAsync_MethodReturnsVoid_ReturnCodeIsCorrect()
    {
        const int expectedReturnCode = 8765;

        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var executor = new CliActionExecutor(serviceProvider);

        var context = CreateTestContext(nameof(ActionExecutorTestClass.Execute));

        context.ReturnCode = expectedReturnCode;

        // Act
        await executor.ExecuteAsync(context);

        // Assert
        context.ReturnCode
            .Should()
            .Be(expectedReturnCode);
    }

    private static CliActionContext CreateTestContext(string methodName)
    {
        var context = new CliActionContext(new CliActionRequest(Array.Empty<string>()))
        {
            ActionRoute = new CliActionRoute(
                typeof(ActionExecutorTestClass),
                typeof(ActionExecutorTestClass).GetMethod(methodName)
                ?? throw new InvalidOperationException(), Array.Empty<string>())
        };

        return context;
    }
}