using System.Security.Claims;
using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;

[UsedImplicitly]
public class TestUserClaimsProvider : IUserClaimsProvider
{
    private readonly IUserClaimsProvider _userClaimsProvider;

    public TestUserClaimsProvider(IUserClaimsProvider userClaimsProvider)
    {
        _userClaimsProvider = userClaimsProvider;
    }

    public Task<IEnumerable<Claim>> GetUserClaimsAsync(string userName, string? domain)
    {
        return _userClaimsProvider.GetUserClaimsAsync(userName, domain);
    }
}