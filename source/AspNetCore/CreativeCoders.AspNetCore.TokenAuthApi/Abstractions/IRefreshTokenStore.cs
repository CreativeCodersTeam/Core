namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

public interface IRefreshTokenStore
{
    Task AddRefreshTokenAsync(string refreshToken, string authToken, DateTimeOffset expire);

    Task RemoveRefreshTokenAsync(string refreshToken);
}
