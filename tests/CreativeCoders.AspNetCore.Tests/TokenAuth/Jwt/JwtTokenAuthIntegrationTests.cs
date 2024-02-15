using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
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
    public async Task Login_WithCredentialsRequestingCookieCustomCookieNameAndPath_ReturnsAuthCookie()
    {
        const string expectedToken = "123456789012";

        // Arrange
        await using var context = new TestServerContext<TestStartup>(null,
            null, x =>
            {
                x.AuthTokenName = "test_auth_token";
                x.CookiePath = "/api";
            });

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
            .Be($"test_auth_token={expectedToken}; path=/api; secure; httponly");
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

    [Fact]
    public async Task Login_WithCredentialsRequestingCookieWithRefreshCookie_ReturnsAuthAndRefreshCookie()
    {
        const string expectedToken = "123456789012";

        // Arrange
        await using var context = new TestServerContext<TestStartup>(null,
            null, x => x.UseRefreshTokens = true);

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

        var cookies = response.Headers.GetValues("set-cookie").ToArray();

        var authCookie = cookies[0];

        authCookie
            .Should()
            .Be($"auth_token={expectedToken}; path=/; secure; httponly");

        var refreshCookie = cookies[1];

        refreshCookie
            .Should()
            .Be($"refresh_auth_token={expectedToken}; path=/; secure; httponly");
    }

    [Fact]
    public async Task Login_WithCredentialsWithRefreshToken_ReturnsAuthAndRefreshTokenInPayload()
    {
        const string expectedAuthToken = "123456789012";
        const string expectedRefreshToken = "24680";

        // Arrange
        await using var context = new TestServerContext<TestStartup>(null, null,
            x => x.UseRefreshTokens = true);

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
            .Returns(Task.FromResult(expectedAuthToken))
            .Once()
            .Then
            .Returns(Task.FromResult(expectedRefreshToken));

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
            .NotBeNull()
            .And
            .HaveCount(2);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        var authToken = content!["auth_token"];

        authToken
            .Should()
            .Be(expectedAuthToken);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        var refreshToken = content!["refresh_auth_token"];

        refreshToken
            .Should()
            .Be(expectedRefreshToken);
    }

    [Fact]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public async Task RefreshToken_WithCredentialsWithRefreshToken_ReturnsAuthAndRefreshTokenInPayload()
    {
        const string expectedAuthToken = "123456789012";
        const string expectedRefreshToken = "24680";
        const string expectedAuthToken2 = "2222123456789012";
        const string expectedRefreshToken2 = "222224680";

        // Arrange
        await using var context = new TestServerContext<TestStartup>(null, null,
            x => x.UseRefreshTokens = true);

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
            .Returns(Task.FromResult(expectedAuthToken))
            .Once()
            .Then
            .Returns(Task.FromResult(expectedRefreshToken))
            .Once()
            .Then
            .Returns(Task.FromResult(expectedAuthToken2))
            .Once()
            .Then
            .Returns(expectedRefreshToken2);

        A.CallTo(() => context.TokenCreator.ReadTokenFromAsync(A<string>.Ignored))
            .Returns(Task.FromResult(new AuthToken
            {
                Claims = new[]
                {
                    new Claim(ClaimTypes.Name, "test")
                }
            }));

        A.CallTo(() =>
                context.UserAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var response = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        var loginResponseContent = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        var refreshToken = loginResponseContent!["refresh_auth_token"];

        var refreshResponse = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/refresh-token", UriKind.Relative),
            JsonContent.Create(
                new RefreshTokenRequest
                {
                    RefreshToken = refreshToken
                }));

        // Assert
        refreshResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var refreshResponseContent =
            await refreshResponse.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        var newAuthToken = refreshResponseContent!["auth_token"];

        newAuthToken
            .Should()
            .Be(expectedAuthToken2);

        var newRefreshToken = refreshResponseContent!["refresh_auth_token"];

        newRefreshToken
            .Should()
            .Be(expectedRefreshToken2);
    }
}
