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
        _userAuthProvider = userAuthProvider;
        _tokenHandler = tokenHandler;
    }

    [Route("RequestToken")]
    [AllowAnonymous]
    [HttpPost]
    public IActionResult RequestToken([FromBody] TokenRequest request)
    {
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
