using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CreativeCoders.AspNetCore.TokenAuth;

public interface IUserClaimsProvider
{
    Task<IEnumerable<Claim>> GetUserClaimsAsync(string userName);
}
