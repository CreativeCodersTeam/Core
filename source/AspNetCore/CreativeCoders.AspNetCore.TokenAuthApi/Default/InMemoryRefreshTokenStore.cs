using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Default;

public class InMemoryRefreshTokenStore : IRefreshTokenStore
{
    private static readonly List<RefreshTokenItem> __items = new List<RefreshTokenItem>();

    public Task AddRefreshTokenAsync(string refreshToken, string authToken, DateTimeOffset expire)
    {
        lock (__items)
        {
            __items.Add(new RefreshTokenItem
            {
                RefreshToken = refreshToken,
                AuthToken = authToken,
                Expire = expire
            });
        }

        return Task.CompletedTask;
    }

    public Task<bool> IsTokenValidAsync(string refreshToken)
    {
        lock (__items)
        {
            var item = __items.Find(x => x.RefreshToken == refreshToken);

            return Task.FromResult(item != null && item.Expire > DateTimeOffset.Now);
        }
    }

    public Task RemoveRefreshTokenAsync(string refreshToken)
    {
        lock (__items)
        {
            __items.RemoveAll(x => x.RefreshToken == refreshToken);
        }

        return Task.CompletedTask;
    }
}
