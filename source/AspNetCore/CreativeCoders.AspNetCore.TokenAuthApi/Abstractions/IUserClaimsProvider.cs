using System.Security.Claims;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuthApi.Abstractions;

[PublicAPI]
public interface IUserClaimsProvider
{
    Task<IEnumerable<Claim>> GetUserClaimsAsync(string userName, string? domain);
}
