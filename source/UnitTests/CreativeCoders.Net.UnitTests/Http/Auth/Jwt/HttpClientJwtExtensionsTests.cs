using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CreativeCoders.Net.Http.Auth.Jwt;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http.Auth.Jwt;

public class HttpClientJwtExtensionsTests
{
    [Fact]
    public async Task RequestJwtTokenAsync_WithEmptyTokenPropertyName_ReturnsFirstPropertyAsToken()
    {
        const string expectedToken = "TestToken";

        var mockHttpClientContext = new MockHttpClientContext();

        mockHttpClientContext
            .Respond()
            .ReturnJson(new {authToken2 = expectedToken, authToken1 = expectedToken + "2" });

        var client = mockHttpClientContext.CreateClient();

        var token = await client
            .RequestJwtTokenAsync(
                new Uri("http://test.com/auth"),
                new JwtTokenRequest
                {
                    UserName = "user",
                    Password = "pass",
                    Domain = "domain"
                });

        Assert.Equal(expectedToken, token.Token);
    }

    [Fact]
    public async Task RequestJwtTokenAsync_WithSpecificTokenPropertyName_ReturnsToken()
    {
        const string expectedToken = "TestToken";
        var expectedTokenRequest = new JwtTokenRequest
        {
            UserName = "user",
            Password = "pass",
            Domain = "domain"
        };
        var mockHttpClientContext = new MockHttpClientContext();

        mockHttpClientContext
            .Respond()
            .ReturnJson(new { specificAuthToken = expectedToken });

        var client = mockHttpClientContext.CreateClient();

        var token = await client
            .RequestJwtTokenAsync(
                new Uri("http://test.com/auth"),
                expectedTokenRequest,
                "specificAuthToken");

        Assert.Equal(expectedToken, token.Token);

        Assert.Single(mockHttpClientContext.RecordedRequests);

        mockHttpClientContext
            .CallShouldBeMade("http://test.com/auth")
            .WithVerb(HttpMethod.Post)
            .WithContentType(ContentMediaTypes.Application.Json)
            .WithContentText(JsonSerializer.Serialize(expectedTokenRequest,
                new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}));
    }

    [Fact]
    public async Task RequestJwtTokenAsync_WithWrongTokenPropertyName_ThrowsException()
    {
        const string expectedToken = "TestToken";
        const string expectedTokenPropertyName = "authToken";
        var expectedResponse = new {otherAuthToken = expectedToken};

        var mockHttpClientContext = new MockHttpClientContext();

        mockHttpClientContext
            .Respond()
            .ReturnJson(expectedResponse);

        var client = mockHttpClientContext.CreateClient();

        var exception = await Assert.ThrowsAsync<JwtTokenNotFoundException>(() => client
            .RequestJwtTokenAsync(
                new Uri("http://test.com/auth"),
                new JwtTokenRequest
                {
                    UserName = "user",
                    Password = "pass",
                    Domain = "domain"
                },
                expectedTokenPropertyName));

        Assert.Equal(expectedTokenPropertyName, exception.TokenPropertyName);
        Assert.Equal(JsonSerializer.Serialize(expectedResponse), exception.Response);
    }
}