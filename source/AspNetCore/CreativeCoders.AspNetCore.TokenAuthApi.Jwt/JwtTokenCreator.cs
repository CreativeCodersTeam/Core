using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Jwt;

public class JwtTokenCreator : ITokenCreator
{
    private readonly SecurityKey? _securityKey;

    public JwtTokenCreator(IOptions<JwtTokenAuthApiOptions> options)
    {
        _securityKey = Ensure.NotNull(options).Value.SecurityKey;

        if (_securityKey == null)
        {
            throw new InvalidOperationException("Security key must not be null");
        }
    }

    public Task<string> CreateTokenAsync(string issuer, string userName, IEnumerable<Claim> claims)
    {
        Ensure.NotNull(userName);

        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            "",
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
