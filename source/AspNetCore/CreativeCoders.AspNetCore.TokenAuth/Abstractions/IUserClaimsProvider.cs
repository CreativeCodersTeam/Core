using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CreativeCoders.AspNetCore.TokenAuth.Abstractions;

public interface IUserClaimsProvider
{
    Task<IEnumerable<Claim>> GetUserClaimsAsync(string userName, string? domain);
}
