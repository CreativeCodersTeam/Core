using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

[PublicAPI]
public interface IUserAuthProvider
{
    Task<bool> AuthenticateAsync(string userName, string password, string? domain);
}
