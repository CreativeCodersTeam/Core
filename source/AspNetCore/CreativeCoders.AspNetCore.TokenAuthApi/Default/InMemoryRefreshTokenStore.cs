using System.Collections.Concurrent;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.Core.Threading;

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

    public Task RemoveRefreshTokenAsync(string refreshToken)
    {
        lock (__items)
        {
            __items.RemoveAll(x => x.RefreshToken == refreshToken);
        }

        return Task.CompletedTask;
    }
}

internal class RefreshTokenItem
{
    public string RefreshToken { get; set; } = string.Empty;

    public string AuthToken { get; set; } = string.Empty;

    public DateTimeOffset Expire { get; set; }
}
