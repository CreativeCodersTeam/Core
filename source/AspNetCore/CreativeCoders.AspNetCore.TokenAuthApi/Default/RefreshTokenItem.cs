namespace CreativeCoders.AspNetCore.TokenAuthApi.Default;

internal class RefreshTokenItem
{
    public string RefreshToken { get; init; } = string.Empty;

    public string AuthToken { get; set; } = string.Empty;

    public DateTimeOffset Expire { get; init; }
}
