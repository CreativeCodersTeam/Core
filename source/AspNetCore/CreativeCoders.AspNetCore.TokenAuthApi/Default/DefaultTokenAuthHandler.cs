using System.Security.Claims;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using CreativeCoders.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Default;

public class DefaultTokenAuthHandler : ITokenAuthHandler
{
    private readonly TokenAuthApiOptions _apiOptions;

    private readonly IRefreshTokenStore _refreshTokenStore;

    private readonly ITokenCreator _tokenCreator;

    private readonly IUserProvider _userProvider;

    public DefaultTokenAuthHandler(IUserProvider userProvider, IRefreshTokenStore refreshTokenStore,
        ITokenCreator tokenCreator, IOptions<TokenAuthApiOptions> options)
    {
        _userProvider = Ensure.NotNull(userProvider);
        _refreshTokenStore = Ensure.NotNull(refreshTokenStore);
        _tokenCreator = Ensure.NotNull(tokenCreator);
        _apiOptions = Ensure.NotNull(options).Value;
    }

    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest, HttpResponse httpResponse)
    {
        Ensure.NotNull(loginRequest);
        Ensure.NotNull(httpResponse);

        if (string.IsNullOrWhiteSpace(loginRequest.UserName) ||
            string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return new UnauthorizedObjectResult(new { error = "Invalid credentials" });
        }

        if (!await _userProvider.AuthenticateAsync(loginRequest.UserName,
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

        return await CreateLoginAuthTokensResponse(loginRequest, httpResponse).ConfigureAwait(false);
    }

    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest,
        HttpRequest httpRequest, HttpResponse httpResponse)
    {
        var useCookies = string.IsNullOrEmpty(refreshTokenRequest.RefreshToken);

        var refreshToken = useCookies
            ? httpRequest.Cookies[_apiOptions.RefreshTokenName]
            : refreshTokenRequest.RefreshToken;

        if (string.IsNullOrEmpty(refreshToken))
        {
            return new UnauthorizedObjectResult(new { error = "Refresh token invalid" });
        }

        if (!await _refreshTokenStore.IsTokenValidAsync(refreshToken).ConfigureAwait(false))
        {
            return new UnauthorizedObjectResult(new { error = "Refresh token invalid" });
        }

        var token = await _tokenCreator.ReadTokenFromStringAsync(refreshToken).ConfigureAwait(false);

        var userName = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userName))
        {
            return new UnauthorizedObjectResult(new { error = "Refresh token invalid" });
        }

        var domain = token.Claims.FirstOrDefault(x => x.Type == "domain")?.Value;

        var response = await CreateLoginAuthTokensResponse(
                new LoginRequest
                {
                    UserName = userName,
                    Domain = domain,
                    UseCookies = useCookies
                }, httpResponse)
            .ConfigureAwait(false);

        await _refreshTokenStore.RemoveRefreshTokenAsync(refreshToken).ConfigureAwait(false);

        return response;
    }

    public Task<IActionResult> LogoutAsync()
    {
        throw new NotSupportedException("Logout is currently not supported");
    }

    private async Task<IActionResult> CreateLoginAuthTokensResponse(LoginRequest loginRequest,
        HttpResponse httpResponse)
    {
        Ensure.IsNotNullOrEmpty(loginRequest.UserName);

        var claims = await _userProvider.GetUserClaimsAsync(loginRequest.UserName, loginRequest.Domain)
            .ConfigureAwait(false);

        var authToken = await _tokenCreator
            .CreateTokenAsync(_apiOptions.Issuer, loginRequest.UserName, claims)
            .ConfigureAwait(false);

        var loginAuthTokens = new LoginAuthTokens
        {
            AuthToken = authToken,
            RefreshToken = _apiOptions.UseRefreshTokens
                ? await CreateRefreshTokenAsync(loginRequest.UserName, loginRequest.Domain, authToken)
                    .ConfigureAwait(false)
                : string.Empty
        };

        return CreateLoginResponse(loginRequest.UseCookies, loginAuthTokens, httpResponse);
    }

    private async Task<string> CreateRefreshTokenAsync(string userName, string? domain,
        string authToken)
    {
        var refreshToken = await _tokenCreator.CreateTokenAsync(_apiOptions.Issuer, userName,
            new[]
            {
                new Claim("auth_token", authToken),
                new Claim(ClaimTypes.Name, userName),
                new Claim("domain", domain ?? string.Empty)
            }).ConfigureAwait(false);

        await _refreshTokenStore.AddRefreshTokenAsync(refreshToken,
            DateTimeOffset.Now.AddDays(7)).ConfigureAwait(false);

        return refreshToken;
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
