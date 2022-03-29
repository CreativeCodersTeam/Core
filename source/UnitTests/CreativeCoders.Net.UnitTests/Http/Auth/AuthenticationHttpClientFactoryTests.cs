using System.Net.Http;
using CreativeCoders.Net.Http.Auth;
using FakeItEasy;
using Microsoft.Extensions.Options;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http.Auth;

public class AuthenticationHttpClientFactoryTests
{
    [Fact]
    public void CreateClient_WithOutParameters_ReturnsClientWithHandlerFromHandlerFactory()
    {
        var messageHandlerFactory = A.Fake<IHttpMessageHandlerFactory>();

        var clientFactory = new AuthenticationHttpClientFactory(messageHandlerFactory);

        var client = clientFactory.CreateClient();

        A.CallTo(() => messageHandlerFactory.CreateHandler(Options.DefaultName))
            .MustHaveHappenedOnceExactly();

        Assert.IsType<AuthenticationHttpClient>(client);
    }

    [Fact]
    public void CreateClient_WithName_ReturnsClientWithHandlerFromHandlerFactory()
    {
        var messageHandlerFactory = A.Fake<IHttpMessageHandlerFactory>();

        var clientFactory = new AuthenticationHttpClientFactory(messageHandlerFactory);

        var client = clientFactory.CreateClient("TestClient");

        A.CallTo(() => messageHandlerFactory.CreateHandler("TestClient")).MustHaveHappenedOnceExactly();

        Assert.IsType<AuthenticationHttpClient>(client);
    }
}
