using System.Threading.Tasks;
using CreativeCoders.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreativeCoders.AspNetCore.TokenAuth;

[Route("auth/[controller]")]
[ApiController]
public class TokenAuthController : ControllerBase
{
    private readonly IUserAuthProvider _userAuthProvider;

    private readonly ITokenHandler _tokenHandler;

    public TokenAuthController(IUserAuthProvider userAuthProvider, ITokenHandler tokenHandler)
    {
        _userAuthProvider = Ensure.NotNull(userAuthProvider);
        _tokenHandler = Ensure.NotNull(tokenHandler);
    }

    [AllowAnonymous]
    [HttpPost("request-token")]
    public async Task<IActionResult> RequestTokenAsync([FromBody] TokenRequest request)
    {
        if (request.UserName == null || request.Password == null)
        {
            return BadRequest("Invalid credentials");
        }

        if (!await _userAuthProvider.CheckUserAsync(request.UserName, request.Password, request.Domain).ConfigureAwait(false))
        {
            return BadRequest("Could not verify username and password");
        }

        var jwt = await _tokenHandler.CreateTokenAsync(request).ConfigureAwait(false);

        return Ok(new
        {
            authToken = jwt
        });
    }
}
