using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CreativeCoders.Net.Http.Auth.Jwt;
using CreativeCoders.UnitTests.Net.Http;
using Xunit;

namespace CreativeCoders.Net.UnitTests.Http.Auth.Jwt;

public class JwtClientTests
{
    [Fact]
    public async Task RequestTokenInfoAsync_SendCorrectRequest_ReturnTokenInfo()
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

        var jwtClient = new JwtClient(mockHttpClientContext.CreateClient());

        var tokenInfo =
            await jwtClient.RequestTokenInfoAsync(new Uri("http://test.com/auth"), expectedTokenRequest);

        Assert.Equal(expectedToken, tokenInfo.Token);

        mockHttpClientContext
            .CallShouldBeMade("http://test.com/auth")
            .WithVerb(HttpMethod.Post)
            .WithContentType(ContentMediaTypes.Application.Json)
            .WithContentText(JsonSerializer.Serialize(expectedTokenRequest,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }

    [Fact]
    public async Task RequestTokenAsync_SendCorrectRequest_ReturnToken()
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

        var jwtClient = new JwtClient(mockHttpClientContext.CreateClient());

        var tokenInfo =
            await jwtClient.RequestTokenAsync(new Uri("http://test.com/auth"), expectedTokenRequest);

        Assert.Equal(expectedToken, tokenInfo);

        mockHttpClientContext
            .CallShouldBeMade("http://test.com/auth")
            .WithVerb(HttpMethod.Post)
            .WithContentType(ContentMediaTypes.Application.Json)
            .WithContentText(JsonSerializer.Serialize(expectedTokenRequest,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}