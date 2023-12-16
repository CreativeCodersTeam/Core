using System.Threading.Tasks;
using CreativeCoders.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.TokenAuth;

[Route("api/[controller]")]
[ApiController]
public class TokenAuthController : ControllerBase
{
    private readonly ITokenAuthHandler _tokenAuthHandler;

    public TokenAuthController(ITokenAuthHandler tokenAuthHandler)
    {
        _tokenAuthHandler = Ensure.NotNull(tokenAuthHandler);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        return _tokenAuthHandler.LoginAsync(loginRequest, Response);
    }

    [HttpPost("refresh-token")]
    public Task<IActionResult> RefreshTokenAsync()
    {
        return _tokenAuthHandler.RefreshTokenAsync();
    }
}
