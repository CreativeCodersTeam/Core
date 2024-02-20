using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Api;

[PublicAPI]
public class LogoutRequest
{
    public string AuthToken { get; set; } = string.Empty;
}
