﻿using System.Net;
using CreativeCoders.UnitTests.Net.Http;

namespace CreativeCoders.UnitTests.UnitTests.Net.Http;

public class MockHttpClientFactoryTests
{
    [Fact]
    public async Task CreateClient_WithMockContextWithFuncCtor_ReturnsMockedData()
    {
        const string expectedContent = "TestData";

        var context = new MockHttpClientContext();

        context
            .Respond()
            .ForUri("http://test.com/")
            .ReturnText(expectedContent, HttpStatusCode.OK);

        var httpClientFactory = new MockHttpClientFactory(context.CreateMessageHandler);

        var client = httpClientFactory.CreateClient(string.Empty);

        var response = await client.GetStringAsync("http://test.com");

        Assert.Equal(expectedContent, response);
    }

    [Fact]
    public async Task CreateClient_WithMockContext_ReturnsMockedData()
    {
        const string expectedContent = "TestData";

        var context = new MockHttpClientContext();

        context
            .Respond()
            .ForUri("http://test.com/")
            .ReturnText(expectedContent, HttpStatusCode.OK);

        var httpClientFactory = new MockHttpClientFactory(context.CreateMessageHandler());

        var client = httpClientFactory.CreateClient(string.Empty);

        var response = await client.GetStringAsync("http://test.com");

        Assert.Equal(expectedContent, response);
    }
}
