using CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.Tests.TokenAuth.Jwt.TestServerSetup;

[UsedImplicitly]
public class TestUserAuthProvider : IUserAuthProvider
{
    private readonly IUserAuthProvider _userAuthProvider;

    public TestUserAuthProvider(IUserAuthProvider userAuthProvider)
    {
        _userAuthProvider = userAuthProvider;
    }

    public Task<bool> AuthenticateAsync(string userName, string password, string? domain)
    {
        return _userAuthProvider.AuthenticateAsync(userName, password, domain);
    }
}