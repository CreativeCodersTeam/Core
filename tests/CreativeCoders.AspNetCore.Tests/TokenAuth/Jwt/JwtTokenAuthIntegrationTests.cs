using System.Net;
using System.Security.Claims;
using System.Text.Json;
using CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

public sealed class JwtTokenAuthIntegrationTests : IDisposable
{
    private readonly TestServerContext<TestStartup> _testContext;

    private readonly ITokenCreator _tokenCreator;

    private readonly IUserAuthProvider _userAuthProvider;

    private readonly IUserClaimsProvider _userClaimsProvider;

    public JwtTokenAuthIntegrationTests()
    {
        _userAuthProvider = A.Fake<IUserAuthProvider>();
        _userClaimsProvider = A.Fake<IUserClaimsProvider>();
        _tokenCreator = A.Fake<ITokenCreator>();

        _testContext =
            new TestServerContext<TestStartup>(services =>
            {
                services.AddScoped<IUserAuthProvider>(_ => _userAuthProvider);
                services.AddScoped<IUserClaimsProvider>(_ => _userClaimsProvider);
                services.AddScoped<ITokenCreator>(_ => _tokenCreator);
            });
    }

    public void Dispose()
    {
        _testContext.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task Login_EmptyCredentials_ReturnsNotAuthorizedStatusCode()
    {
        // Arrange
        var loginRequest = new LoginRequest();

        // Act
        var response = await _testContext.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithCredentialsRequestingCookie_ReturnsAuthCookie()
    {
        const string expectedToken = "123456789012";

        // Arrange
        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com",
            UseCookies = true
        };

        A.CallTo(() =>
                _tokenCreator.CreateTokenAsync(A<string>.Ignored, A<string>.Ignored,
                    A<IEnumerable<Claim>>.Ignored))
            .Returns(Task.FromResult(expectedToken));

        A.CallTo(() =>
                _userAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var response = await _testContext.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var cookie = response.Headers.GetValues("set-cookie").First();

        cookie
            .Should()
            .Be($"auth_token={expectedToken}; path=/; secure; httponly");
    }

    [Fact]
    public async Task Login_WithCredentials_ReturnsAuthTokenInPayload()
    {
        const string expectedToken = "123456789012";

        // Arrange
        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com",
            UseCookies = false
        };

        A.CallTo(() =>
                _tokenCreator.CreateTokenAsync(A<string>.Ignored, A<string>.Ignored,
                    A<IEnumerable<Claim>>.Ignored))
            .Returns(Task.FromResult(expectedToken));

        A.CallTo(() =>
                _userAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var response = await _testContext.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var content = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        var token = content.RootElement.GetProperty("auth_token").GetString();

        token
            .Should()
            .Be(expectedToken);
    }
}
