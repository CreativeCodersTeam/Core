namespace CreativeCoders.Net.Http.Auth.Jwt;

public class JwtTokenInfo
{
    public JwtTokenInfo(string token)
    {
        Token = token;
    }

    public string Token { get; }
}
