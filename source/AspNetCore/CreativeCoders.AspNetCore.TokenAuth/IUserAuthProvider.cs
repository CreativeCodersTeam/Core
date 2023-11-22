using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuth;

[PublicAPI]
public interface IUserAuthProvider
{
    Task<bool> CheckUserAsync(string userName, string password, string? domain);
}
