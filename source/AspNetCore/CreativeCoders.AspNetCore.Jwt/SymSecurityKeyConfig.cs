namespace CreativeCoders.AspNetCore.Jwt;

public class SymSecurityKeyConfig : ISymSecurityKeyConfig
{
    public SymSecurityKeyConfig(string securityKey)
    {
        SecurityKey = securityKey;
    }

    public string SecurityKey { get; }
}
