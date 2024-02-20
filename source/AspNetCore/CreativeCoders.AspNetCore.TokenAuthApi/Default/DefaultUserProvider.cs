using System.Security.Claims;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using CreativeCoders.Core;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Default;

public class DefaultUserProvider : IUserProvider
{
    private readonly IUserAuthProvider _userAuthProvider;

    private readonly IUserClaimsProvider _userClaimsProvider;

    public DefaultUserProvider(IUserAuthProvider userAuthProvider, IUserClaimsProvider userClaimsProvider)
    {
        _userAuthProvider = Ensure.NotNull(userAuthProvider);
        _userClaimsProvider = Ensure.NotNull(userClaimsProvider);
    }

    public Task<bool> AuthenticateAsync(string userName, string password, string? domain)
    {
        return _userAuthProvider.AuthenticateAsync(userName, password, domain);
    }

    public Task<IEnumerable<Claim>> GetUserClaimsAsync(string userName, string? domain)
    {
        return _userClaimsProvider.GetUserClaimsAsync(userName, domain);
    }
}
