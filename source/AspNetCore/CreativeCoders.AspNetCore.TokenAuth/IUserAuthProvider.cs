namespace CreativeCoders.AspNetCore.TokenAuth;

public interface IUserAuthProvider
{
    bool CheckUser(string userName, string password, string domain);
}