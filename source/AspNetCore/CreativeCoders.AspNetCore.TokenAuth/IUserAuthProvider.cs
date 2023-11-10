using JetBrains.Annotations;

namespace CreativeCoders.AspNetCore.TokenAuth;

[PublicAPI]
public interface IUserAuthProvider
{
    bool CheckUser(string userName, string password, string? domain);
}
