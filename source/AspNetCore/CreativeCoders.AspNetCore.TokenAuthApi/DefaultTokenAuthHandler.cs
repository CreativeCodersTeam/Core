using System.Security.Claims;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using CreativeCoders.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CreativeCoders.AspNetCore.TokenAuthApi;

public class DefaultTokenAuthHandler : ITokenAuthHandler
{
    private readonly TokenAuthApiOptions _apiOptions;

    private readonly ITokenCreator _tokenCreator;

    private readonly IUserAuthProvider _userAuthProvider;

    private readonly IUserClaimsProvider _userClaimsProvider;

    public DefaultTokenAuthHandler(IUserAuthProvider userAuthProvider, IUserClaimsProvider userClaimsProvider,
        ITokenCreator tokenCreator, IOptions<TokenAuthApiOptions> options)
    {
        _userAuthProvider = Ensure.NotNull(userAuthProvider);
        _userClaimsProvider = Ensure.NotNull(userClaimsProvider);
        _tokenCreator = Ensure.NotNull(tokenCreator);
        _apiOptions = Ensure.NotNull(options).Value;
    }

    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest, HttpResponse httpResponse)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.UserName) ||
            string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return new UnauthorizedObjectResult(new { error = "Invalid credentials" });
        }

        if (!await _userAuthProvider.AuthenticateAsync(loginRequest.UserName,
                    loginRequest.Password,
                    loginRequest.Domain)
                .ConfigureAwait(false))
        {
            return new UnauthorizedObjectResult(
                new
                {
                    error = "Invalid login attempt. Please check your credentials and try again."
                });
        }

        var claims = await _userClaimsProvider.GetUserClaimsAsync(loginRequest.UserName, loginRequest.Domain)
            .ConfigureAwait(false);

        var authToken = await _tokenCreator
            .CreateTokenAsync(_apiOptions.Issuer, loginRequest.UserName, claims)
            .ConfigureAwait(false);

        var loginAuthTokens = new LoginAuthTokens
        {
            AuthToken = authToken,
            RefreshToken = _apiOptions.UseRefreshTokens
                ? await CreateRefreshTokenAsync(loginRequest.UserName, authToken).ConfigureAwait(false)
                : string.Empty
        };

        return CreateLoginResponse(loginRequest.UseCookies, loginAuthTokens, httpResponse);
    }

    public Task<IActionResult> RefreshTokenAsync()
    {
        throw new NotSupportedException("Refresh is currently not supported");
    }

    public Task<IActionResult> LogoutAsync()
    {
        throw new NotSupportedException("Logout is currently not supported");
    }

    private Task<string> CreateRefreshTokenAsync(string userName, string authToken)
    {
        return _tokenCreator.CreateTokenAsync(_apiOptions.Issuer, userName,
            new[] { new Claim("auth_token", authToken) });
    }

    private IActionResult CreateLoginResponse(bool useCookies, LoginAuthTokens tokens,
        HttpResponse httpResponse)
    {
        return useCookies
            ? CreateLoginResponseWithCookies(tokens, httpResponse)
            : CreateLoginResponseAsJson(tokens);
    }

    private IActionResult CreateLoginResponseWithCookies(LoginAuthTokens tokens, HttpResponse httpResponse)
    {
        httpResponse.Cookies.Append(_apiOptions.AuthTokenName, tokens.AuthToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Domain = _apiOptions.CookieDomain,
            Path = _apiOptions.CookiePath
        });

        httpResponse.Cookies.Append(_apiOptions.RefreshTokenName, tokens.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Domain = _apiOptions.RefreshCookieDomain,
            Path = _apiOptions.RefreshCookiePath
        });

        return new OkResult();
    }

    private IActionResult CreateLoginResponseAsJson(LoginAuthTokens tokens)
    {
        if (string.IsNullOrEmpty(tokens.RefreshToken))
        {
            return new OkObjectResult(new Dictionary<string, string>
                { { _apiOptions.AuthTokenName, tokens.AuthToken } });
        }

        return new OkObjectResult(new Dictionary<string, string>
        {
            { _apiOptions.AuthTokenName, tokens.AuthToken },
            { _apiOptions.RefreshTokenName, tokens.RefreshToken }
        });
    }
}
