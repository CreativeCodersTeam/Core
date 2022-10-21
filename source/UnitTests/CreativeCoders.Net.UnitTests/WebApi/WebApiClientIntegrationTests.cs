using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CreativeCoders.Net.UnitTests.WebApi.TestData;
using CreativeCoders.Net.WebApi;
using CreativeCoders.UnitTests.Net.Http;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CreativeCoders.Net.UnitTests.WebApi;

public class WebApiClientIntegrationTests
{
    [Fact]
    public async Task GetItemAsync_ApiClientIsCalled_DemoItemIsReturnedByClient()
    {
        const string expectedText = "TestText";

        var context = new MockHttpClientContext();

        context
            .Respond()
            .ForUri("http://demo.app/one-item")
            .ReturnText(JsonSerializer.Serialize(new DemoItem {Text = expectedText}), HttpStatusCode.OK);

        var httpClientFactory = A.Fake<IHttpClientFactory>();

        A
            .CallTo(() => httpClientFactory.CreateClient(Options.DefaultName))
            .Returns(context.CreateClient());

        var webApi = CreateServiceProvider(httpClientFactory).GetRequiredService<IDemoWebApi>();

        // Act
        var item = await webApi.GetItemAsync();

        // Assert
        item
            .Should()
            .NotBeNull();

        item.Text
            .Should()
            .Be(expectedText);
    }

    [Fact]
    public async Task GetItemsAsync_ApiClientIsCalled_DemoItemsIsReturnedByClient()
    {
        const string expectedText0 = "TestText0";
        const string expectedText1 = "TestText1";

        var context = new MockHttpClientContext();

        var expectedItems = JsonSerializer.Serialize(
            CreateDemoItems(expectedText0, expectedText1));

        context
            .Respond()
            .ForUri("http://demo.app/items")
            .ReturnText(expectedItems, HttpStatusCode.OK);

        var httpClientFactory = A.Fake<IHttpClientFactory>();

        A
            .CallTo(() => httpClientFactory.CreateClient(Options.DefaultName))
            .Returns(context.CreateClient());

        var webApi = CreateServiceProvider(httpClientFactory).GetRequiredService<IDemoWebApi>();

        // Act
        var items =
            (await webApi.GetItemsAsync().ConfigureAwait(false))
            .ToArray();

        // Assert
        items
            .Should()
            .NotBeNull();

        items
            .Should()
            .HaveCount(2);

        items[0].Text
            .Should()
            .Be(expectedText0);

        items[1].Text
            .Should()
            .Be(expectedText1);
    }

    [Fact]
    public async Task GetItemAsync_WithUrlArgumentApiClientIsCalled_DemoItemIsReturnedByClient()
    {
        const string expectedItemId = "1234";

        var context = new MockHttpClientContext();

        context
            .Respond()
            .ForUri($"http://demo.app/item/{expectedItemId}")
            .ReturnText(JsonSerializer.Serialize(new DemoItem {Text = expectedItemId}), HttpStatusCode.OK);

        var httpClientFactory = A.Fake<IHttpClientFactory>();

        A
            .CallTo(() => httpClientFactory.CreateClient(Options.DefaultName))
            .Returns(context.CreateClient());

        var webApi = CreateServiceProvider(httpClientFactory).GetRequiredService<IDemoWebApi>();

        // Act
        var item = await webApi.GetItemAsync(expectedItemId);

        // Assert
        item
            .Should()
            .NotBeNull();

        item.Text
            .Should()
            .Be(expectedItemId);
    }

    private static IEnumerable<DemoItem> CreateDemoItems(params string[] textItems)
    {
        return textItems.Select(x => new DemoItem {Text = x});
    }

    private static IServiceProvider CreateServiceProvider(IHttpClientFactory httpClientFactory)
    {
        var services = new ServiceCollection();

        services.AddSingleton(httpClientFactory);

        services.AddWebApiClient<IDemoWebApi>("http://demo.app/");

        return services.BuildServiceProvider();
    }
}
