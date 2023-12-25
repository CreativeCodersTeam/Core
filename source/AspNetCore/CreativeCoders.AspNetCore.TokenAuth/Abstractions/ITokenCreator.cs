using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CreativeCoders.AspNetCore.TokenAuth.Abstractions;

public interface ITokenCreator
{
    Task<string> CreateTokenAsync(string issuer, string userName, IEnumerable<Claim> claims);
}
