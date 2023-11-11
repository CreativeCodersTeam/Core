using System;
using System.Threading.Tasks;
using CreativeCoders.Net.Http;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http;

public class HttpClientPostTextExtensionsTests
{
    [Fact]
    public async Task PostTextAsync_WithDefaultMediaType_RequestWithSpecifiedTextIsMade()
    {
        const string expectedData = "TestData";

        var mockHttpClientContext = new MockHttpClientContext();

        mockHttpClientContext
            .Respond()
            .ReturnText("Test");

        var client = mockHttpClientContext.CreateClient();

        _ = await client.PostTextAsync(new Uri("http://test.com"), expectedData);

        Assert.Single(mockHttpClientContext.RecordedRequests);

        mockHttpClientContext
            .CallShouldBeMade("http://test.com/")
            .WithContentType(ContentMediaTypes.Text.Plain)
            .WithContentText(expectedData);
    }

    [Fact]
    public async Task PostTextAsync_WithXmlMediaType_RequestWithSpecifiedTextIsMade()
    {
        const string expectedData = "TestData";

        var mockHttpClientContext = new MockHttpClientContext();

        mockHttpClientContext
            .Respond()
            .ReturnText("Test");

        var client = mockHttpClientContext.CreateClient();

        _ = await client.PostTextAsync(new Uri("http://test.com"), expectedData,
            ContentMediaTypes.Text.Xml);

        Assert.Single(mockHttpClientContext.RecordedRequests);

        mockHttpClientContext
            .CallShouldBeMade("http://test.com/")
            .WithContentType(ContentMediaTypes.Text.Xml)
            .WithContentText(expectedData);
    }
}
