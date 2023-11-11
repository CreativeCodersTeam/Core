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

    [Route("RequestToken")]
    [AllowAnonymous]
    [HttpPost]
    public IActionResult RequestToken([FromBody] TokenRequest request)
    {
        if (request.UserName == null || request.Password == null)
        {
            return BadRequest("Invalid credentials");
        }

        if (!_userAuthProvider.CheckUser(request.UserName, request.Password, request.Domain))
        {
            return BadRequest("Could not verify username and password");
        }

        var jwt = _tokenHandler.CreateToken(request);

        return Ok(new
        {
            authToken = jwt
        });
    }
}
