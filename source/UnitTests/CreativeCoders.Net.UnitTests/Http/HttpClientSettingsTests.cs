using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using CreativeCoders.Net.Http;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http;

[SuppressMessage("ReSharper", "ConvertToLocalFunction")]
public class HttpClientSettingsTests
{
    [Fact]
    public void Add_ConfigureAction_ActionIsAddedToHttpClientActions()
    {
        HttpClientFactoryOptions configuredOptions = null;

        var httpClientSettings = new HttpClientSettings();

        Action<HttpClientFactoryOptions> configureAction = x => configuredOptions = x;

        // Act
        httpClientSettings.Add("Test", configureAction);

        // Assert
        var options = new HttpClientFactoryOptions();

        httpClientSettings.Get("Test")(options);

        configuredOptions
            .Should()
            .BeSameAs(options);
    }

    [Fact]
    public void Add_ConfigureClientUseConfigureAction_ActionIsAddedToHttpClientActions()
    {
        var httpClientSettings = new HttpClientSettings();

        Action<HttpClient> configureAction = _ => { };

        // Act
        httpClientSettings.Add("Test").ConfigureClient(configureAction);

        // Assert
        var options = new HttpClientFactoryOptions();

        httpClientSettings.Get("Test")(options);

        options.HttpClientActions
            .Should()
            .HaveCount(1);

        options.HttpClientActions.First()
            .Should()
            .BeSameAs(configureAction);
    }

    [Fact]
    public void Add_ConfigureClientHandlerUseConfigureAction_ActionIsAddedToHttpClientActions()
    {
        var httpClientSettings = new HttpClientSettings();

        var httpClientHandler = A.Fake<HttpClientHandler>();

        var configureFunc = () => httpClientHandler;

        var httpClientHandlerBuilder = A.Fake<HttpMessageHandlerBuilder>();

        // Act
        httpClientSettings.Add("Test").ConfigureClientHandler(configureFunc);

        // Assert
        var options = new HttpClientFactoryOptions();

        httpClientSettings.Get("Test")(options);

        options.HttpMessageHandlerBuilderActions
            .Should()
            .HaveCount(1);

        options.HttpMessageHandlerBuilderActions.First()(httpClientHandlerBuilder);

        httpClientHandlerBuilder.PrimaryHandler
            .Should()
            .BeSameAs(httpClientHandler);
    }
}
