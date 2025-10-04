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
public class HttpClientSettingsTests
{
    [Fact]
    public void Add_ConfigureAction_ActionIsAddedToHttpClientActions()
    {
        HttpClientFactoryOptions configuredOptions = null;

        var httpClientSettings = new HttpClientSettings() as IHttpClientSettings;

        // Act
        httpClientSettings.Add("Test", ConfigureAction);

        // Assert
        var options = new HttpClientFactoryOptions();

        httpClientSettings.Get("Test")?.Invoke(options);

        configuredOptions
            .Should()
            .BeSameAs(options);
        return;

        void ConfigureAction(HttpClientFactoryOptions x) => configuredOptions = x;
    }

    [Fact]
    public void Add_ConfigureClientUseConfigureAction_ActionIsAddedToHttpClientActions()
    {
        var httpClientSettings = new HttpClientSettings() as IHttpClientSettings;

        Action<HttpClient> configureAction = _ => { };

        // Act
        httpClientSettings.Add("Test").ConfigureClient(configureAction);

        // Assert
        var options = new HttpClientFactoryOptions();

        httpClientSettings.Get("Test")?.Invoke(options);

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
        var httpClientSettings = new HttpClientSettings() as IHttpClientSettings;

        var httpClientHandler = A.Fake<HttpClientHandler>();

        var configureFunc = () => httpClientHandler;

        var httpClientHandlerBuilder = A.Fake<HttpMessageHandlerBuilder>();

        // Act
        httpClientSettings.Add("Test").ConfigureClientHandler(configureFunc);

        // Assert
        var options = new HttpClientFactoryOptions();

        httpClientSettings.Get("Test")?.Invoke(options);

        options.HttpMessageHandlerBuilderActions
            .Should()
            .HaveCount(1);

        options.HttpMessageHandlerBuilderActions.First()(httpClientHandlerBuilder);

        httpClientHandlerBuilder.PrimaryHandler
            .Should()
            .BeSameAs(httpClientHandler);
    }
}
