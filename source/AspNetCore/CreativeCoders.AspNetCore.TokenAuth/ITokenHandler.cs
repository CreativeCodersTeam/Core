namespace CreativeCoders.AspNetCore.TokenAuth
{
    public interface ITokenHandler
    {
        string CreateToken(TokenRequest request);
    }
}