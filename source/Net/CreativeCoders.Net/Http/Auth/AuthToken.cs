using Newtonsoft.Json;

namespace CreativeCoders.Net.Http.Auth
{
    public class AuthToken
    {
        public AuthToken(string token)
        {
            Token = token;
        }

        [JsonProperty("authToken")]
        public string Token { get; set; }
    }
}