using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.AspNetCore.TokenAuthApi.Api;
using CreativeCoders.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CreativeCoders.AspNetCore.TokenAuthApi;

public class DefaultTokenAuthHandler : ITokenAuthHandler
{
    private readonly IUserAuthProvider _userAuthProvider;

    private readonly IUserClaimsProvider _userClaimsProvider;

    private readonly ITokenCreator _tokenCreator;

    private readonly TokenAuthApiOptions _apiOptions;

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

        var token = await _tokenCreator.CreateTokenAsync(_apiOptions.Issuer, loginRequest.UserName, claims)
            .ConfigureAwait(false);

        if (_apiOptions.UseCookies)
        {
            httpResponse.Cookies.Append(_apiOptions.AuthTokenName, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Domain = _apiOptions.CookieDomain
            });
        }
        else
        {
            return new OkObjectResult(new Dictionary<string, string> { { _apiOptions.AuthTokenName, token } });
        }

        return new OkResult();
    }

    public Task<IActionResult> RefreshTokenAsync()
    {
        throw new NotSupportedException("Refresh is currently not supported");
    }

    public Task<IActionResult> LogoutAsync()
    {
        throw new NotSupportedException("Logout is currently not supported");
    }
}
