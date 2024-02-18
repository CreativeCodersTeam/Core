using System.Net.Mime;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Api;

[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class TokenAuthController : ControllerBase
{
    private readonly ITokenAuthHandler _tokenAuthHandler;

    public TokenAuthController(ITokenAuthHandler tokenAuthHandler)
    {
        _tokenAuthHandler = Ensure.NotNull(tokenAuthHandler);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        Ensure.NotNull(loginRequest);

        if (string.IsNullOrWhiteSpace(loginRequest.UserName) ||
            string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return Unauthorized(new { error = "Invalid credentials" });
        }

        return await _tokenAuthHandler.LoginAsync(loginRequest, Response).ConfigureAwait(false);
    }

    [HttpPost("refresh-token")]
    public Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        return _tokenAuthHandler.RefreshTokenAsync(refreshTokenRequest, Request, Response);
    }

    [HttpPost("logout")]
    public Task<IActionResult> LogoutAsync(LogoutRequest logoutRequest)
    {
        return _tokenAuthHandler.LogoutAsync(logoutRequest, Request, Response);
    }
}
