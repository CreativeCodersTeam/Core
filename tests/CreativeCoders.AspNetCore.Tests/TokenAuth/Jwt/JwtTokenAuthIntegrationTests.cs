using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

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
            .Be($"cc_auth_token={expectedToken}; path=/; secure; httponly");
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
        var token = content!["cc_auth_token"];

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
            .Be($"cc_auth_token={expectedToken}; path=/; secure; httponly");

        var refreshCookie = cookies[1];

        refreshCookie
            .Should()
            .Be($"cc_refresh_auth_token={expectedToken}; path=/; secure; httponly");
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
        var authToken = content!["cc_auth_token"];

        authToken
            .Should()
            .Be(expectedAuthToken);

        // ReSharper disable once NullableWarningSuppressionIsUsed
        var refreshToken = content["cc_refresh_auth_token"];

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

        A.CallTo(() => context.TokenCreator.ReadTokenFromStringAsync(A<string>.Ignored))
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

        var refreshToken = loginResponseContent!["cc_refresh_auth_token"];

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

        var newAuthToken = refreshResponseContent!["cc_auth_token"];

        newAuthToken
            .Should()
            .Be(expectedAuthToken2);

        var newRefreshToken = refreshResponseContent["cc_refresh_auth_token"];

        newRefreshToken
            .Should()
            .Be(expectedRefreshToken2);
    }

    [Fact]
    public async Task RefreshToken_EmptyRefreshToken_ReturnUnauthorizedResult()
    {
        // Arrange
        await using var context = new TestServerContext<TestStartup>();

        var refreshTokenRequest = new RefreshTokenRequest();

        // Act
        var response = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/refresh-token", UriKind.Relative),
            JsonContent.Create(refreshTokenRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_InvalidRefreshToken_ReturnUnauthorizedResult()
    {
        // Arrange
        var refreshTokenStore = A.Fake<IRefreshTokenStore>();

        A.CallTo(() => refreshTokenStore.IsTokenValidAsync(A<string>.Ignored))
            .Returns(false);

        await using var context = new TestServerContext<TestStartup>(
            services => services.AddSingleton(refreshTokenStore));

        var refreshTokenRequest = new RefreshTokenRequest
        {
            RefreshToken = "1234567890"
        };

        // Act
        var response = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/refresh-token", UriKind.Relative),
            JsonContent.Create(refreshTokenRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_RefreshTokenHasNoUser_ReturnUnauthorizedResult()
    {
        const string refreshToken = "1234567890";

        // Arrange
        var refreshTokenStore = A.Fake<IRefreshTokenStore>();

        A.CallTo(() => refreshTokenStore.IsTokenValidAsync(A<string>.Ignored))
            .Returns(true);

        await using var context = new TestServerContext<TestStartup>(
            services => services.AddSingleton(refreshTokenStore));

        A.CallTo(() => context.TokenCreator.ReadTokenFromStringAsync(refreshToken))
            .Returns(new AuthToken());

        var refreshTokenRequest = new RefreshTokenRequest
        {
            RefreshToken = refreshToken
        };

        // Act
        var response = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/refresh-token", UriKind.Relative),
            JsonContent.Create(refreshTokenRequest));

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logout_AfterLogin_ReturnsOk()
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
        var loginResponse = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        var content = await loginResponse.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        // ReSharper disable once NullableWarningSuppressionIsUsed
        var authToken = content!["cc_auth_token"];

        var logoutRequest = new LogoutRequest
        {
            AuthToken = authToken
        };

        var logoutResponse = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/logout", UriKind.Relative), JsonContent.Create(logoutRequest));

        // Assert
        logoutResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Logout_EmptyAuthToken_ReturnsUnauthorized()
    {
        // Arrange
        await using var context = new TestServerContext<TestStartup>(null,
            null, x => x.UseRefreshTokens = true);

        // Act
        var logoutRequest = new LogoutRequest();

        var logoutResponse = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/logout", UriKind.Relative), JsonContent.Create(logoutRequest));

        // Assert
        logoutResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public async Task CallEndpointWithAuthorization_LoginBeforeAndSendToken_RequestSucceeded()
    {
        // Arrange
        const string authTokenName = "this_is_a_token";

        await using var context =
            new TestServerContext<TestStartup>(
                x => x.AddScoped<ITokenCreator, JwtTokenCreator>(),
                null, x =>
                {
                    x.AuthTokenName = authTokenName;
                    x.Issuer = "my Issuer";
                },
                x =>
                {
                    x.AuthTokenName = authTokenName;
                    x.Issuer = "my Issuer";
                });

        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com",
            UseCookies = false
        };

        A.CallTo(() =>
                context.UserAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var response = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        var content = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        var httpRequestMsg =
            new HttpRequestMessage(HttpMethod.Get, new Uri("api/test/get-data", UriKind.Relative));

        var authToken = content![authTokenName];

        httpRequestMsg.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", authToken);

        var callResponse = await context.HttpClient.SendAsync(httpRequestMsg);

        // Assert
        callResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var callContent = await callResponse.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        callContent
            .Should()
            .NotBeNull();

        var data = callContent!["testProp"];

        data
            .Should()
            .Be("TestValue1");
    }

    [Fact]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public async Task
        CallEndpointWithAuthorization_LoginBeforeThenRefreshTokenAndSendThatToken_RequestSucceeded()
    {
        // Arrange
        await using var context =
            new TestServerContext<TestStartup>(
                x => x.AddScoped<ITokenCreator, JwtTokenCreator>(),
                null, x => x.UseRefreshTokens = true);

        var loginRequest = new LoginRequest
        {
            UserName = "test",
            Password = "password",
            Domain = "example.com",
            UseCookies = false
        };

        A.CallTo(() =>
                context.UserAuthProvider.AuthenticateAsync(loginRequest.UserName, loginRequest.Password,
                    loginRequest.Domain))
            .Returns(Task.FromResult(true));

        // Act
        var loginResponse = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/login", UriKind.Relative), JsonContent.Create(loginRequest));

        var content = await loginResponse.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        var refreshToken = content!["cc_refresh_auth_token"];

        var refreshResponse = await context.HttpClient.PostAsync(
            new Uri("api/TokenAuth/refresh-token", UriKind.Relative), JsonContent.Create(
                new RefreshTokenRequest
                {
                    RefreshToken = refreshToken
                }));

        var refreshContent = await refreshResponse.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        var authToken = refreshContent!["cc_auth_token"];

        var httpRequestMsg =
            new HttpRequestMessage(HttpMethod.Get, new Uri("api/test/get-data", UriKind.Relative));

        httpRequestMsg.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", authToken);

        var callResponse = await context.HttpClient.SendAsync(httpRequestMsg);

        // Assert
        callResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var callContent = await callResponse.Content.ReadFromJsonAsync<IDictionary<string, string>>();

        callContent
            .Should()
            .NotBeNull();

        var data = callContent!["testProp"];

        data
            .Should()
            .Be("TestValue1");
    }
}
