using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt;

[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
public sealed class JwtTokenAuthIntegrationTests
{
    [Fact]
    public async Task Login_EmptyCredentials_ReturnsNotAuthorizedStatusCode()
    {
        // Arrange
        await using var context = new TestServerContext<TestStartup>();

        var loginRequest = new LoginRequest();

        // Act
        var response = await context.HttpClient.PostAsync(
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
        await using var context = new TestServerContext<TestStartup>();

        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com",
            UseCookies = true
        };

        A.CallTo(() =>
                context.TokenCreator.CreateTokenAsync(A<string>.Ignored, A<string>.Ignored,
                    A<IEnumerable<Claim>>.Ignored))
            .Returns(Task.FromResult(expectedToken));

        A.CallTo(() =>
                context.UserAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var response = await context.HttpClient.PostAsync(
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
    public async Task Login_WithCredentialsRequestingCookieCustomCookieName_ReturnsAuthCookie()
    {
        const string expectedToken = "123456789012";

        // Arrange
        await using var context = new TestServerContext<TestStartup>(_ => { },
            null, x => x.AuthTokenName = "test_auth_token");

        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com",
            UseCookies = true
        };

        A.CallTo(() =>
                context.TokenCreator.CreateTokenAsync(A<string>.Ignored, A<string>.Ignored,
                    A<IEnumerable<Claim>>.Ignored))
            .Returns(Task.FromResult(expectedToken));

        A.CallTo(() =>
                context.UserAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var response = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var cookie = response.Headers.GetValues("set-cookie").First();

        cookie
            .Should()
            .Be($"test_auth_token={expectedToken}; path=/; secure; httponly");
    }

    [Fact]
    public async Task Login_WithCredentials_ReturnsAuthTokenInPayload()
    {
        const string expectedToken = "123456789012";

        // Arrange
        await using var context = new TestServerContext<TestStartup>();

        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com",
            UseCookies = false
        };

        A.CallTo(() =>
                context.TokenCreator.CreateTokenAsync(A<string>.Ignored, A<string>.Ignored,
                    A<IEnumerable<Claim>>.Ignored))
            .Returns(Task.FromResult(expectedToken));

        A.CallTo(() =>
                context.UserAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var response = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        content
            .Should()
            .NotBeNull();

        // ReSharper disable once NullableWarningSuppressionIsUsed
        var token = content!["auth_token"];

        token
            .Should()
            .Be(expectedToken);
    }
}
