using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.Core.Text;
using CreativeCoders.Net.JsonRpc;
using CreativeCoders.UnitTests.Net.Http;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Net.UnitTests.JsonRpc;

[Collection("Env")]
public class JsonRpcClientTests
{
    [Fact]
    public async Task ExecuteAsync_ValidRequest_ReturnsExpectedResponse()
    {
        // Arrange
        var env = A.Fake<IEnvironment>();

        A.CallTo(() => env.TickCount).Returns(1234);

        using var _ = Env.SetEnvironmentImpl(env);

        var url = new Uri("https://example.com");
        const string methodName = "TestMethod";
        var arguments = new object[] { 1, "test" };

        var rpcCallResponse = new JsonRpcResponse<string>
        {
            Result = "TestResult",
            Id = 1234
        };

        var httpContext = new MockHttpClientContext();

        httpContext
            .Respond()
            .ForUri(url.ToString())
            .WithVerb(HttpMethod.Post)
            .ReturnJson(rpcCallResponse, HttpStatusCode.OK);

        var httpClient = httpContext.CreateClient();
        var jsonRpcClient = new JsonRpcClient(httpClient);

        var response = await jsonRpcClient.ExecuteAsync<string>(url, methodName, arguments).ConfigureAwait(false);

        // Assert
        response.Result
            .Should()
            .Be("TestResult");

        httpContext
            .CallShouldBeMade(url.ToString())
            .WithContentText(new JsonRpcRequest(methodName, arguments).ToJson(new JsonSerializerOptions(JsonSerializerDefaults.Web)));
    }

    [Fact]
    public async Task ExecuteAsync_NullResponse_ThrowsInvalidOperationException()
    {
        // Arrange
        var url = new Uri("https://example.com");
        const string methodName = "TestMethod";
        var arguments = new object[] { 1, "test" };

        var httpClient = A.Fake<HttpClient>();
        var jsonRpcClient = new JsonRpcClient(httpClient);

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(null, typeof(JsonRpcResponse<int>))
        };

        A.CallTo(() => httpClient.SendAsync(A<HttpRequestMessage>.Ignored, A<CancellationToken>.Ignored))
            .Returns(httpResponse);

        // Act & Assert
        await jsonRpcClient
            .Awaiting(x => x.ExecuteAsync<int>(url, methodName, arguments))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .ConfigureAwait(false);
    }


    [Fact]
    public async Task ExecuteAsync_IdMismatch_ThrowsInvalidOperationException()
    {
        // Arrange
        var env = A.Fake<IEnvironment>();

        A.CallTo(() => env.TickCount).Returns(12);

        using var _ = Env.SetEnvironmentImpl(env);

        var url = new Uri("https://example.com");
        const string methodName = "TestMethod";
        var arguments = new object[] { 1, "test" };

        var rpcCallResponse = new JsonRpcResponse<string>
        {
            Result = "TestResult",
            Id = 1234
        };

        var httpContext = new MockHttpClientContext();

        httpContext
            .Respond()
            .ForUri(url.ToString())
            .WithVerb(HttpMethod.Post)
            .ReturnJson(rpcCallResponse, HttpStatusCode.OK);

        var httpClient = httpContext.CreateClient();
        var jsonRpcClient = new JsonRpcClient(httpClient);

        // Act & Assert
        await jsonRpcClient
            .Awaiting(x => x.ExecuteAsync<string>(url, methodName, arguments))
            .Should()
            .ThrowAsync<InvalidOperationException>().ConfigureAwait(false);
    }
}
