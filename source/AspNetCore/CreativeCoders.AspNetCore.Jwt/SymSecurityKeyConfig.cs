using CreativeCoders.Core;

namespace CreativeCoders.AspNetCore.Jwt;

public class SymSecurityKeyConfig : ISymSecurityKeyConfig
{
    public SymSecurityKeyConfig(string securityKey)
    {
        SecurityKey = Ensure.IsNotNullOrEmpty(securityKey);
    }

    public string SecurityKey { get; }
}
