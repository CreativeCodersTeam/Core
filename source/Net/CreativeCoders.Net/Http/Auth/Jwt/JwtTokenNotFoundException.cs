using System;

namespace CreativeCoders.Net.Http.Auth.Jwt
{
    public class JwtTokenNotFoundException : Exception
    {
        internal JwtTokenNotFoundException(string tokenPropertyName, string response)
            : base($"Jwt token not found in property '{tokenPropertyName}'")
        {
            TokenPropertyName = tokenPropertyName;
            Response = response;
        }

        public string Response { get; }

        public string TokenPropertyName { get; set; }
    }
}