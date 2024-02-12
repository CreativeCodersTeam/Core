using System.Security.Claims;
using CreativeCoders.AspNetCore.TokenAuthApi;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using CreativeCoders.AspNetCore.TokenAuthApi.Default;
using CreativeCoders.UnitTests;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.Tests.TokenAuthApi;

public class DefaultTokenAuthHandlerTests
{
    private readonly DefaultTokenAuthHandler _handler;

    private readonly TokenAuthApiOptions _options;

    private readonly IRefreshTokenStore _refreshTokenStore;

    private readonly ITokenCreator _tokenCreator;

    private readonly IUserProvider _userProvider;

    public DefaultTokenAuthHandlerTests()
    {
        _userProvider = A.Fake<IUserProvider>();
        _refreshTokenStore = A.Fake<IRefreshTokenStore>();
        _tokenCreator = A.Fake<ITokenCreator>();

        _options = new TokenAuthApiOptions
        {
            Issuer = "testIssuer",
            AuthTokenName = "authToken",
            CookieDomain = "cookieDomain"
        };

        _handler = new DefaultTokenAuthHandler(_userProvider, _refreshTokenStore, _tokenCreator,
            Options.Create(_options));
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ReturnsBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            UserName = "",
            Password = ""
        };

        // Act
        var result =
            await _handler.LoginAsync(loginRequest, A.Fake<HttpResponse>());

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task LoginAsync_InvalidLoginAttempt_ReturnsUnauthorizedRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            UserName = "user",
            Password = "pass"
        };
        A.CallTo(() =>
                _userProvider.AuthenticateAsync(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored))
            .Returns(false);

        // Act
        var result =
            await _handler.LoginAsync(loginRequest, A.Fake<HttpResponse>());

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task LoginAsync_WithCookies_ReturnsOkObjectResultIfCookiesOff()
    {
        // Arrange
        var loginRequest = SetupSuccessfulLogin();
        var httpResponse = A.Fake<HttpResponse>();

        // Act
        var result = await _handler.LoginAsync(loginRequest, httpResponse);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task LoginAsync_WithCookies_SetsCookieIfCookiesOn()
    {
        // Arrange
        var loginRequest = SetupSuccessfulLogin();
        loginRequest.UseCookies = true;

        var httpResponse = A.Fake<HttpResponse>();

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Domain = _options.CookieDomain,
        };

        // Act
        await _handler.LoginAsync(loginRequest, httpResponse);

        // Assert
        A.CallTo(() => httpResponse.Cookies.Append(_options.AuthTokenName, "token",
                A<CookieOptions>.That.Matches(x => x.IsEquivalentTo(cookieOptions))))
            .MustHaveHappenedOnceExactly();
    }

    private LoginRequest SetupSuccessfulLogin()
    {
        var loginRequest = new LoginRequest
        {
            UserName = "user",
            Password = "pass"
        };

        A.CallTo(() =>
                _userProvider.AuthenticateAsync(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored))
            .Returns(true);
        A.CallTo(() => _userProvider.GetUserClaimsAsync(A<string>.Ignored, A<string>.Ignored))
            .Returns(new List<Claim>());
        A.CallTo(() =>
            _tokenCreator.CreateTokenAsync(A<string>.Ignored, A<string>.Ignored,
                A<IEnumerable<Claim>>.Ignored)).Returns("token");

        return loginRequest;
    }
}
