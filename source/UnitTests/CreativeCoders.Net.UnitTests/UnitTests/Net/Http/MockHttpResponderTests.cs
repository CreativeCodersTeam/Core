using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.UnitTests.Net.Http;

public class MockHttpResponderTests
{
    [Fact]
    public async Task Execute_NoReturn_ThrowsException()
    {
        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com")
        };

        var mockResponse = new MockHttpResponder();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            mockResponse.Execute(requestMessage, CancellationToken.None)).ConfigureAwait(false);
    }

    [Fact]
    public async Task Execute_OnlyReturnText_AlwaysReturnTheSame()
    {
        const string expectedContent = "TestData";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse.ReturnText(expectedContent);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedContent, await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedStatusCode, response.StatusCode);

        requestMessage.RequestUri = new Uri("https://nic.com");
        var secondResponse = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedContent, await secondResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedStatusCode, secondResponse.StatusCode);
    }

    [Fact]
    public async Task Execute_ReturnTextWithStatusCode_AlwaysReturnTheSameAndStatusCode()
    {
        const string expectedContent = "TestData";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.Accepted;

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse.ReturnText(expectedContent, expectedStatusCode);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedContent, await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedStatusCode, response.StatusCode);

        requestMessage.RequestUri = new Uri("https://nic.com");
        var secondResponse = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedContent, await secondResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedStatusCode, secondResponse.StatusCode);
    }

    [Fact]
    public async Task Execute_ReturnTextForUriMatching_ReturnsMockData()
    {
        const string expectedContent = "TestData";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse
            .ForUri("http://test.com/")
            .ReturnText(expectedContent, expectedStatusCode);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedContent, await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Fact]
    public async Task Execute_ReturnTextForUriNotMatching_ReturnsNull()
    {
        const string expectedContent = "TestData";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://nic.com")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse
            .ForUri("http://test.com/")
            .ReturnText(expectedContent, expectedStatusCode);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Null(response);
    }

    [Fact]
    public async Task Execute_ReturnTextForVerb_ReturnsMockData()
    {
        const string expectedContent = "TestData";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get
        };

        var mockResponse = new MockHttpResponder();

        mockResponse
            .WithVerb(HttpMethod.Get)
            .ReturnText(expectedContent, expectedStatusCode);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedContent, await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Fact]
    public async Task Execute_ReturnTextForVerbNotMatching_ReturnsNull()
    {
        const string expectedContent = "TestData";
        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post
        };

        var mockResponse = new MockHttpResponder();

        mockResponse
            .WithVerb(HttpMethod.Get)
            .ReturnText(expectedContent, expectedStatusCode);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Null(response);
    }

    [Fact]
    public async Task Execute_ReturnJson_ReturnsCorrectJsonData()
    {
        var expectedData = new {Text = "TestText"};

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse.ReturnJson(expectedData);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(
            JsonSerializer.Serialize(expectedData,
                new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}),
            await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Execute_ReturnJsonWithStatusCode_ReturnsCorrectJsonDataAndStatusCode()
    {
        const HttpStatusCode expectedStatusCode = HttpStatusCode.Accepted;
        var expectedData = new {Text = "TestText"};

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse.ReturnJson(expectedData, expectedStatusCode);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedStatusCode, response.StatusCode);
        Assert.Equal(
            JsonSerializer.Serialize(expectedData,
                new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}),
            await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Execute_ReturnJson_ReturnsCorrectContentType()
    {
        var expectedData = new {Text = "TestText"};

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse.ReturnJson(expectedData);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(ContentMediaTypes.Application.Json, response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Then_SecondRequestForSameUri_RespondedBySecondResponder()
    {
        const string expectedContent = "TestData";
        const string expectedSecondContent = "TestData2";

        const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
        const HttpStatusCode expectedSecondStatusCode = HttpStatusCode.OK;

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com/")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse
            .ForUri("http://test.com/")
            .ReturnText(expectedContent, expectedStatusCode);

        mockResponse
            .Then()
            .ReturnText(expectedSecondContent, expectedSecondStatusCode);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedContent, await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedStatusCode, response.StatusCode);

        requestMessage.RequestUri = new Uri("http://test.com/");
        var secondResponse = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedSecondContent,
            await secondResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedSecondStatusCode, secondResponse.StatusCode);
    }

    [Fact]
    public async Task Then_SecondRequestForSameUriWithDifferentRequestBetween_RespondedBySecondResponder()
    {
        const string expectedContent = "TestData";
        const string expectedSecondContent = "TestData2";

        const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
        const HttpStatusCode expectedSecondStatusCode = HttpStatusCode.OK;

        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri("http://test.com/")
        };

        var mockResponse = new MockHttpResponder();

        mockResponse
            .ForUri("http://test.com/")
            .ReturnText(expectedContent, expectedStatusCode);

        mockResponse
            .Then()
            .ReturnText(expectedSecondContent, expectedSecondStatusCode);

        var response = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedContent, await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedStatusCode, response.StatusCode);

        requestMessage.RequestUri = new Uri("http://test1.com/");
        var otherResponse = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Null(otherResponse);

        requestMessage.RequestUri = new Uri("http://test.com/");
        var secondResponse = await mockResponse.Execute(requestMessage, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.Equal(expectedSecondContent,
            await secondResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
        Assert.Equal(expectedSecondStatusCode, secondResponse.StatusCode);
    }
}
