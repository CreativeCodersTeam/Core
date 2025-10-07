using System;
using System.Net.Http;
using CreativeCoders.Net.Http;
using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http;

public class DynamicHttpClientFactoryOptionsServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDynamicHttpClient_AddHttpClientSettings_HttpClientHasConfiguredBaseAddress()
    {
        var expectedUrl = new Uri("https://test.com");

        var services = new ServiceCollection();

        services.AddDynamicHttpClient();

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<IHttpClientSettings>()
            .Add("Test")
            .ConfigureClient(x => x.BaseAddress = expectedUrl);

        // Act
        var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("Test");

        // Assert
        httpClient.BaseAddress
            .Should()
            .BeSameAs(expectedUrl);
    }
}
