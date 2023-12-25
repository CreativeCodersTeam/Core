using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuth.Abstractions;

[PublicAPI]
public interface IUserAuthProvider
{
    Task<bool> AuthenticateAsync(string userName, string password, string? domain);
}
