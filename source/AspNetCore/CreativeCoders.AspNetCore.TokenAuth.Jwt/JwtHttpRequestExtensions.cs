using Microsoft.AspNetCore.Http;

namespace CreativeCoders.AspNetCore.TokenAuth.Jwt;

public static class JwtHttpRequestExtensions
{
    public static string GetJwtToken(this HttpRequest request, string authTokenName)
    {
        var token =
            request.Headers.Authorization.FirstOrDefault(x =>
                x?.StartsWith("Bearer ", StringComparison.Ordinal) == true);

        if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer ", StringComparison.Ordinal))
        {
            token = token["Bearer ".Length..];
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            token = request.Cookies[authTokenName];
        }

        return token ?? string.Empty;
    }
}
