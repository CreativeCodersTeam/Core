using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http.Auth.Jwt;

[PublicAPI]
public interface IJwtClient
{
    Task<string> RequestTokenAsync(Uri requestUri, JwtTokenRequest tokenRequest);

    Task<JwtTokenInfo> RequestTokenInfoAsync(Uri requestUri, JwtTokenRequest tokenRequest);
}