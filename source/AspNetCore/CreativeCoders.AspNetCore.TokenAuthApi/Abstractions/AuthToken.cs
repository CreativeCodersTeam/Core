using System.Security.Claims;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

public class AuthToken
{
    public IEnumerable<Claim> Claims { get; init; } = Array.Empty<Claim>();
}
