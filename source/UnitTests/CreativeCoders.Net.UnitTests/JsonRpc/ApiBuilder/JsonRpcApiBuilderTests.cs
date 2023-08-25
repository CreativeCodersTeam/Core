using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.JsonRpc;
using CreativeCoders.Net.JsonRpc.ApiBuilder;
using CreativeCoders.Net.UnitTests.JsonRpc.TestData;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Net.UnitTests.JsonRpc.ApiBuilder;

public class JsonRpcApiBuilderTests
{
    [Fact]
    public async Task Build_UrlIsGiven_TheJsonRpcCallIsMadeViaJsonRpcClient()
    {
        const string argStringValue = "StringValue";
        const int argIntValue = 1245;
        var apiUrl = new Uri("http://test.test/api/json");

        // Arrange
        var jsonRpcClientFactory = A.Fake<IJsonRpcClientFactory>();

        var jsonClient = A.Fake<IJsonRpcClient>();

        A.CallTo(() => jsonRpcClientFactory.Create()).Returns(jsonClient);

        var apiBuilder = new JsonRpcApiBuilder<ITestJsonRpcApi>(
            new ProxyBuilder<ITestJsonRpcApi>(new ProxyGenerator()),
            jsonRpcClientFactory);

        // Act
        var api = apiBuilder.ForUrl(apiUrl).Build();

        await api.TestMethod(argStringValue, argIntValue).ConfigureAwait(false);

        // Assert
        A
            .CallTo(() =>
                jsonClient.ExecuteAsync<string>(
                    apiUrl, nameof(ITestJsonRpcApi.TestMethod), argStringValue, argIntValue))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Build_NoUrlIsGiven_ThrowsException()
    {
        // Arrange
        var apiBuilder = new JsonRpcApiBuilder<ITestJsonRpcApi>(
            new ProxyBuilder<ITestJsonRpcApi>(new ProxyGenerator()),
            A.Fake<IJsonRpcClientFactory>());

        // Act
        var act = () => apiBuilder.Build();

        // Assert
        act
            .Should()
            .Throw<InvalidOperationException>();
    }
}
