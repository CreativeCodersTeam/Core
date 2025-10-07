using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using CreativeCoders.Net.Http;
using FakeItEasy;
using AwesomeAssertions;
using Microsoft.Extensions.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http;

[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class HttpClientSetupTests
{
    [Fact]
    public void ConfigureClient_UseConfigureAction_ActionIsAddedToHttpClientActions()
    {
        var httpClientSetup = new HttpClientSetup();

        Action<HttpClient> configureAction = _ => { };

        // Act
        httpClientSetup.ConfigureClient(configureAction);

        // Assert
        var options = new HttpClientFactoryOptions();

        httpClientSetup.InvokeSetup(options);

        options.HttpClientActions
            .Should()
            .HaveCount(1);

        options.HttpClientActions.First()
            .Should()
            .BeSameAs(configureAction);
    }

    [Fact]
    public void ConfigureClientHandler_UseConfigureAction_ActionIsAddedToHttpClientActions()
    {
        var httpClientSetup = new HttpClientSetup();

        var httpClientHandler = A.Fake<HttpClientHandler>();

        var configureFunc = () => httpClientHandler;

        var httpClientHandlerBuilder = A.Fake<HttpMessageHandlerBuilder>();

        // Act
        httpClientSetup.ConfigureClientHandler(configureFunc);

        // Assert
        var options = new HttpClientFactoryOptions();

        httpClientSetup.InvokeSetup(options);

        options.HttpMessageHandlerBuilderActions
            .Should()
            .HaveCount(1);

        options.HttpMessageHandlerBuilderActions.First()(httpClientHandlerBuilder);

        httpClientHandlerBuilder.PrimaryHandler
            .Should()
            .BeSameAs(httpClientHandler);
    }
}
