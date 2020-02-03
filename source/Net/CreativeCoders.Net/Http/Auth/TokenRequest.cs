namespace CreativeCoders.Net.Http.Auth
{
    public class TokenRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Domain { get; set; }
    }
}