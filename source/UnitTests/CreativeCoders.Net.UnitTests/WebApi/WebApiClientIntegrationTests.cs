using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
            .WithVerb(HttpMethod.Get)
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
            .WithVerb(HttpMethod.Get)
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
            .WithVerb(HttpMethod.Get)
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

    [Fact]
    public async Task CreateItemAsync_WithItemAsArgument_NewDemoItemIsReturnedByClient()
    {
        const string expectedText = "1234";

        var context = new MockHttpClientContext();

        context
            .Respond()
            .WithVerb(HttpMethod.Post)
            .ForUri("http://demo.app/create")
            .ReturnText(JsonSerializer.Serialize(new DemoItem {Text = $"{expectedText}:{expectedText}"}), HttpStatusCode.OK);

        var httpClientFactory = A.Fake<IHttpClientFactory>();

        var newItem = new DemoItem {Text = expectedText};

        A
            .CallTo(() => httpClientFactory.CreateClient(Options.DefaultName))
            .Returns(context.CreateClient());

        var webApi = CreateServiceProvider(httpClientFactory).GetRequiredService<IDemoWebApi>();

        // Act
        var item = await webApi.CreateItemAsync(newItem);

        // Assert
        item
            .Should()
            .NotBeNull();

        item.Text
            .Should()
            .Be($"{expectedText}:{expectedText}");

        context
            .CallShouldBeMade("http://demo.app/create")
            .WithVerb(HttpMethod.Post)
            .WithContentText(JsonSerializer.Serialize(newItem));
    }

    [Fact]
    public async Task GetWithOneHeaderAsync_OneHeaderSet_OneHeaderIsSentWithApiCall()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .WithVerb(HttpMethod.Get)
            .ForUri("http://demo.app/one-header")
            .ReturnText("", HttpStatusCode.OK);

        var httpClientFactory = A.Fake<IHttpClientFactory>();

        A
            .CallTo(() => httpClientFactory.CreateClient(Options.DefaultName))
            .Returns(context.CreateClient());

        var webApi = CreateServiceProvider(httpClientFactory).GetRequiredService<IDemoWebApi>();

        // Act
        await webApi.GetWithOneHeaderAsync();

        // Assert
        context
            .CallShouldBeMade("http://demo.app/one-header")
            .RequestMeets(x =>
                x.Headers.Count() == 1 &&
                x.Headers.Count(CheckOneHeader) == 1,
                "Header is set correct");
    }

    [Fact]
    public async Task GetWithTwoHeadersAsync_TwoHeadersSet_TwoHeadersAreSentWithApiCall()
    {
        var context = new MockHttpClientContext();

        context
            .Respond()
            .WithVerb(HttpMethod.Get)
            .ForUri("http://demo.app/two-headers")
            .ReturnText("", HttpStatusCode.OK);

        var httpClientFactory = A.Fake<IHttpClientFactory>();

        A
            .CallTo(() => httpClientFactory.CreateClient(Options.DefaultName))
            .Returns(context.CreateClient());

        var webApi = CreateServiceProvider(httpClientFactory).GetRequiredService<IDemoWebApi>();

        // Act
        await webApi.GetWithTwoHeadersAsync();

        // Assert
        context
            .CallShouldBeMade("http://demo.app/two-headers")
            .RequestMeets(x => CheckTwoHeaders(x.Headers),
                "Header is set correct");
    }

    [Fact]
    public async Task GetItemResponseAsync_ApiClientIsCalled_DemoItemIsReturnedWithResponseByClient()
    {
        const string expectedText = "TestText";

        var context = new MockHttpClientContext();

        context
            .Respond()
            .WithVerb(HttpMethod.Get)
            .ForUri("http://demo.app/misc/item-with-response")
            .ReturnText(JsonSerializer.Serialize(new DemoItem {Text = expectedText}), HttpStatusCode.OK);

        var httpClientFactory = A.Fake<IHttpClientFactory>();

        A
            .CallTo(() => httpClientFactory.CreateClient(Options.DefaultName))
            .Returns(context.CreateClient());

        var webApi = CreateServiceProvider(httpClientFactory).GetRequiredService<IDemoWebApi>();

        // Act
        using var response = await webApi.GetItemResponseAsync();

        // Assert
        response
            .Should()
            .NotBeNull();

        response.ResponseMessage.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var data = response.Data;

        data.Text
            .Should()
            .Be(expectedText);

        response.Data
            .Should()
            .BeSameAs(data);
    }

    private static bool CheckTwoHeaders(HttpRequestHeaders httpHeaders)
    {
        var headers = httpHeaders.ToArray();

        return headers.Length == 2 &&
               headers.Count(x =>
                   x.Key == "TheHeader" &&
                   x.Value.Count() == 1 &&
                   x.Value.Any(headerValue => headerValue == "HeaderValueOne")) == 1 &&
               headers.Count(x =>
                   x.Key == "ThatHeader" &&
                   x.Value.Count() == 1 &&
                   x.Value.Any(headerValue => headerValue == string.Empty)) == 1;
    }

    private static bool CheckOneHeader(KeyValuePair<string, IEnumerable<string>> header)
    {
        return header.Key == "TestHeader" &&
               header.Value.Count() == 1 &&
               header.Value.Count(x => x == "HeaderValue0") == 1;
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
