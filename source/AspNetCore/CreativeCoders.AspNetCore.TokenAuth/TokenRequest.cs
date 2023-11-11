using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuth;

[PublicAPI]
public class TokenRequest
{
    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Domain { get; set; }
}
