using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CreativeCoders.AspNetCore.TokenAuth.Abstractions;
using CreativeCoders.Core;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

public class JwtTokenCreator : ITokenCreator
{
    private readonly ISymSecurityKeyConfig _symSecurityKeyConfig;

    public JwtTokenCreator(ISymSecurityKeyConfig symSecurityKeyConfig)
    {
        _symSecurityKeyConfig = Ensure.NotNull(symSecurityKeyConfig);
    }

    public Task<string> CreateTokenAsync(string issuer, string userName, IEnumerable<Claim> claims)
    {
        Ensure.NotNull(userName);

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_symSecurityKeyConfig.SecurityKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            "",
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
