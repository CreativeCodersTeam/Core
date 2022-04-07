using System.Net;
using System.Net.Http;
using CreativeCoders.Net.Http;

namespace CreativeCoders.Net.Avm;

public class FritzBoxConnections : IFritzBoxConnections
{
    private readonly IHttpClientSettings _httpClientSettings;

    public FritzBoxConnections(IHttpClientSettings httpClientSettings)
    {
        _httpClientSettings = httpClientSettings;
    }

    public void Add(string name, FritzBoxConnection options)
    {
        _httpClientSettings
            .Add(name)
            .ConfigureClient(x => x.BaseAddress = options.Url)
            .ConfigureClientHandler(() =>
            {
                var handler = new HttpClientHandler();

                if (!string.IsNullOrEmpty(options.UserName) || !string.IsNullOrEmpty(options.Password))
                {
                    handler.Credentials = new NetworkCredential(options.UserName, options.Password);
                }

                if (options.AllowUntrustedCertificates)
                {
                    handler.ServerCertificateCustomValidationCallback =
                        (_, _, _, _) => true;
                }

                return handler;
            });
    }
}
