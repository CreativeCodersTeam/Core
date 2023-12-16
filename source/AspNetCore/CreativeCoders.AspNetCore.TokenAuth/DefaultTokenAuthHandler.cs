using System.Threading.Tasks;
using CreativeCoders.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CreativeCoders.AspNetCore.TokenAuth;

public class DefaultTokenAuthHandler : ITokenAuthHandler
{
    private readonly IUserAuthProvider _userAuthProvider;

    private readonly IUserClaimsProvider _userClaimsProvider;

    private readonly ITokenCreator _tokenCreator;

    private readonly TokenAuthOptions _options;

    public DefaultTokenAuthHandler(IUserAuthProvider userAuthProvider, IUserClaimsProvider userClaimsProvider,
        ITokenCreator tokenCreator, IOptions<TokenAuthOptions> options)
    {
        _userAuthProvider = Ensure.NotNull(userAuthProvider);
        _userClaimsProvider = Ensure.NotNull(userClaimsProvider);
        _tokenCreator = Ensure.NotNull(tokenCreator);
        _options = Ensure.NotNull(options).Value;
    }

    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest, HttpResponse httpResponse)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.UserName) ||
            string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return new BadRequestObjectResult(new { error = "Invalid credentials" });
        }

        if (!await _userAuthProvider.CheckUserAsync(loginRequest.UserName,
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

        var claims = await _userClaimsProvider.GetUserClaimsAsync(loginRequest.UserName)
            .ConfigureAwait(false);

        // TODO make issuer configurable
        var token = await _tokenCreator.CreateTokenAsync("Issuer", loginRequest.UserName, claims)
            .ConfigureAwait(false);

        if (_options.UseCookies)
        {
            httpResponse.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                // TODO only allow specific domains
                //Domain = ,
                //Path =
            });
        }

        if (!_options.UseCookies)
        {
            return new OkObjectResult(new { jwt = token });
        }

        return new OkResult();
    }

    public Task<IActionResult> RefreshTokenAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<IActionResult> LogoutAsync()
    {
        throw new System.NotImplementedException();
    }
}
