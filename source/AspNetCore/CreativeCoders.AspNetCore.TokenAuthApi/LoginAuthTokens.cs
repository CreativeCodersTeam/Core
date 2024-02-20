namespace CreativeCoders.AspNetCore.TokenAuthApi;

internal class LoginAuthTokens
{
    public string AuthToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;
}
