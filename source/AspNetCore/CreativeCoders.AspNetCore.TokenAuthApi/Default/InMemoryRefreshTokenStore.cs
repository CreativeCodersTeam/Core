using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Default;

public class InMemoryRefreshTokenStore : IRefreshTokenStore
{
    private readonly List<RefreshTokenItem> _items = new List<RefreshTokenItem>();

    public Task AddRefreshTokenAsync(string authToken, string refreshToken, DateTimeOffset expire)
    {
        lock (_items)
        {
            _items.Add(new RefreshTokenItem
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
        lock (_items)
        {
            var item = _items.Find(x => x.RefreshToken == refreshToken);

            return Task.FromResult(item != null && item.Expire > DateTimeOffset.Now);
        }
    }

    public Task RemoveRefreshTokenAsync(string refreshToken)
    {
        lock (_items)
        {
            _items.RemoveAll(x => x.RefreshToken == refreshToken);
        }

        return Task.CompletedTask;
    }

    public Task RemoveRefreshTokenForAuthAsync(string authToken)
    {
        lock (_items)
        {
            _items.RemoveAll(x => x.AuthToken == authToken);
        }

        return Task.CompletedTask;
    }
}
