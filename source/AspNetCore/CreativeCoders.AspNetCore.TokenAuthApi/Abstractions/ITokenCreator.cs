using System.Security.Claims;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

public interface ITokenCreator
{
    Task<string> CreateTokenAsync(string issuer, string userName, IEnumerable<Claim> claims);

    Task<AuthToken> ReadTokenFromAsync(string token);
}

public class AuthToken
{
    public IEnumerable<Claim> Claims { get; init; }
}
