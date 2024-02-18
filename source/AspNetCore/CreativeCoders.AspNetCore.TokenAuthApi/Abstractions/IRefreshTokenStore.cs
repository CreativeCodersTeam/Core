namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

public interface IRefreshTokenStore
{
    Task AddRefreshTokenAsync(string refreshToken, DateTimeOffset expire);

    Task<bool> IsTokenValidAsync(string refreshToken);

    Task RemoveRefreshTokenAsync(string refreshToken);
}
