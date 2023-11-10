using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CreativeCoders.AspNetCore.TokenAuth;
using CreativeCoders.Core;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.Jwt;

public class JwtTokenHandler : ITokenHandler
{
    private readonly ISymSecurityKeyConfig _symSecurityKeyConfig;

    public JwtTokenHandler(ISymSecurityKeyConfig symSecurityKeyConfig)
    {
        _symSecurityKeyConfig = Ensure.NotNull(symSecurityKeyConfig);
    }

    public string CreateToken(TokenRequest request)
    {
        Ensure.NotNull(request.UserName);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.UserName)
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_symSecurityKeyConfig.SecurityKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            request.Domain,
            request.Domain,
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
