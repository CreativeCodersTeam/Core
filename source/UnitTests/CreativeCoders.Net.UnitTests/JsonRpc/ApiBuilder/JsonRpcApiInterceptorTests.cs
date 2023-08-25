using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using CreativeCoders.Net.JsonRpc;
using CreativeCoders.Net.JsonRpc.ApiBuilder;
using CreativeCoders.Net.UnitTests.JsonRpc.TestData;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Net.UnitTests.JsonRpc.ApiBuilder;

public class JsonRpcApiInterceptorTests
{
    [Fact]
    public async Task ExecuteMethod_WithTaskOfTResult_ReturnsResult()
    {
        // Arrange
        const string expectedResult = "422";
        var jsonRpcClient = A.Fake<IJsonRpcClient>();
        var url = new Uri("http://localhost:1234");
        var interceptor = new JsonRpcApiInterceptor<ITestJsonRpcApi>(jsonRpcClient, url);

        var args = new object[] {"TestArg", 1234 };

        A
            .CallTo(() => jsonRpcClient.ExecuteAsync<string>(url, nameof(ITestJsonRpcApi.TestMethod),
                A<object[]>.That.Matches(x => x[0] == args[0] && x[1] == args[1])))
            .Returns(Task.FromResult(new JsonRpcResponse<string> { Result = expectedResult }));

        var invocation = A.Fake<IInvocation>();

        A.CallTo(() => invocation.Method)
            .Returns(typeof(ITestJsonRpcApi).GetMethod(nameof(ITestJsonRpcApi.TestMethod)));

        A.CallTo(() => invocation.Arguments)
            .Returns(args);

        // Act
        interceptor.Intercept(invocation);

        var result = await ((Task<string>)invocation.ReturnValue).ConfigureAwait(false);

        // Assert
        result
            .Should()
            .Be(expectedResult);
    }

    [Fact]
    public async Task ExecuteMethod_WithTaskOfJsonRpcResponse_ReturnsResult()
    {
        // Arrange
        const string expectedResult = "422";
        var jsonRpcClient = A.Fake<IJsonRpcClient>();
        var url = new Uri("http://localhost:1234");
        var interceptor = new JsonRpcApiInterceptor<ITestJsonRpcApi>(jsonRpcClient, url);

        var args = new object[] {"TestArg", 1234 };

        A
            .CallTo(() => jsonRpcClient.ExecuteAsync<string>(url, nameof(ITestJsonRpcApi.TestMethodWithJsonRpcResponse),
                A<object[]>.That.Matches(x => x[0] == args[0] && x[1] == args[1])))
            .Returns(Task.FromResult(new JsonRpcResponse<string> { Result = expectedResult }));

        var invocation = A.Fake<IInvocation>();

        A.CallTo(() => invocation.Method)
            .Returns(typeof(ITestJsonRpcApi).GetMethod(nameof(ITestJsonRpcApi.TestMethodWithJsonRpcResponse)));

        A.CallTo(() => invocation.Arguments)
            .Returns(args);

        // Act
        interceptor.Intercept(invocation);

        var rpcResponse = await ((Task<JsonRpcResponse<string>>)invocation.ReturnValue).ConfigureAwait(false);

        // Assert
        rpcResponse.Result
            .Should()
            .Be(expectedResult);
    }

    [Fact]
    public void ExecuteMethod_InvalidReturnType_ThrowsException()
    {
        // Arrange
        var interceptor = new JsonRpcApiInterceptor<ITestJsonRpcApi>(A.Fake<IJsonRpcClient>(), new Uri("http://localhost:1234"));

        var invocation = A.Fake<IInvocation>();

        A.CallTo(() => invocation.Method)
            .Returns(typeof(ITestJsonRpcApi).GetMethod(nameof(ITestJsonRpcApi.InvalidMethod)));

        // Act & Assert
        interceptor
            .Invoking(x => x.Intercept(invocation))
            .Should()
            .Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task ExecuteMethod_WithIncludeParameterNames_CallsMadeWithParameterNames()
    {
        // Arrange
        const string expectedResult = "422";
        var jsonRpcClient = A.Fake<IJsonRpcClient>();
        var url = new Uri("http://localhost:1234");
        var interceptor = new JsonRpcApiInterceptor<ITestJsonRpcApiWithParamNames>(jsonRpcClient, url);

        var args = new object[] {"TestArg", 1234 };

        A
            .CallTo(() => jsonRpcClient.ExecuteAsync<string>(url, nameof(ITestJsonRpcApi.TestMethod),
                A<object[]>.That.Matches(x => (string)x[0] == "arg1" && x[1] == args[0] && (string)x[2] == "arg2" && x[3] == args[1])))
            .Returns(Task.FromResult(new JsonRpcResponse<string> { Result = expectedResult }));

        var invocation = A.Fake<IInvocation>();

        A.CallTo(() => invocation.Method)
            .Returns(typeof(ITestJsonRpcApi).GetMethod(nameof(ITestJsonRpcApi.TestMethod)));

        A.CallTo(() => invocation.Arguments)
            .Returns(args);

        // Act
        interceptor.Intercept(invocation);

        var result = await ((Task<string>)invocation.ReturnValue).ConfigureAwait(false);

        // Assert
        result
            .Should()
            .Be(expectedResult);
    }

    [Fact]
    public async Task ExecuteMethod_WithCustomNames_CallsMadeWithCustomNames()
    {
        // Arrange
        const string expectedResult = "422";
        var jsonRpcClient = A.Fake<IJsonRpcClient>();
        var url = new Uri("http://localhost:1234");
        var interceptor = new JsonRpcApiInterceptor<ITestJsonRpcApiWithParamNames>(jsonRpcClient, url);

        var args = new object[] {"TestArg", 1234 };

        A
            .CallTo(() => jsonRpcClient.ExecuteAsync<string>(url, "test",
                A<object[]>.That.Matches(x => (string)x[0] == "argument1" && x[1] == args[0] && (string)x[2] == "arg2" && x[3] == args[1])))
            .Returns(Task.FromResult(new JsonRpcResponse<string> { Result = expectedResult }));

        var invocation = A.Fake<IInvocation>();

        A.CallTo(() => invocation.Method)
            .Returns(typeof(ITestJsonRpcApi).GetMethod(nameof(ITestJsonRpcApi.TestMethodWithNames)));

        A.CallTo(() => invocation.Arguments)
            .Returns(args);

        // Act
        interceptor.Intercept(invocation);

        var result = await ((Task<string>)invocation.ReturnValue).ConfigureAwait(false);

        // Assert
        result
            .Should()
            .Be(expectedResult);
    }
}
