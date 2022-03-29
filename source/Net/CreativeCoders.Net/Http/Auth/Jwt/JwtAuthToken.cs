using Newtonsoft.Json;

namespace CreativeCoders.Net.Http.Auth.Jwt;

public class JwtAuthToken
{
    public JwtAuthToken(string token)
    {
        Token = token;
    }

    [JsonProperty("authToken")] public string Token { get; set; }
}
