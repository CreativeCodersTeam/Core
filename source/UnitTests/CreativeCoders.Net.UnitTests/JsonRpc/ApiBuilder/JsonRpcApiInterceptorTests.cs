using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Castle.DynamicProxy.Internal;
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
        var expectedResult = "422";
        var jsonRpcClient = A.Fake<IJsonRpcClient>();
        var url = new Uri("http://localhost:1234");
        var interceptor = new JsonRpcApiInterceptor<ITestJsonRpcApi>(jsonRpcClient, url);

        var args = new object[] {"TestArg", 1234 };

        A
            .CallTo(() => jsonRpcClient.ExecuteAsync<string>(url, "TestMethod",
                A<object[]>.That.Matches(x => x[0] == args[0] && x[1] == args[1])))
            .Returns(Task.FromResult(new JsonRpcResponse<string> { Result = expectedResult }));

        var invocation = A.Fake<IInvocation>();

        A.CallTo(() => invocation.Method)
            .Returns(typeof(ITestJsonRpcApi).GetMethod("TestMethod"));

        A.CallTo(() => invocation.Arguments)
            .Returns(args);

        // Act
        interceptor.Intercept(invocation);

        var result = await ((Task<string>)invocation.ReturnValue).ConfigureAwait(false);

        // Assert
        result.Should().Be(expectedResult);
    }
}
