using JetBrains.Annotations;

namespace CreativeCoders.Net.Http.Auth.Jwt;

[PublicAPI]
public class JwtTokenRequest
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string Domain { get; set; }
}
