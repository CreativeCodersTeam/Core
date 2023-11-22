using System.Threading.Tasks;

namespace CreativeCoders.AspNetCore.TokenAuth;

public interface ITokenHandler
{
    Task<string> CreateTokenAsync(TokenRequest request);
}
