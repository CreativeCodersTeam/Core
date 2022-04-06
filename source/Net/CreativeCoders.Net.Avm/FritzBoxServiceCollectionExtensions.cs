using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using CreativeCoders.Core;
using CreativeCoders.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Net.Avm;

public static class FritzBoxServiceCollectionExtensions
{
    public static void AddFritzBox(this IServiceCollection services,
        Action<FritzBoxConnection> configureOptions)
    {
        Ensure.NotNull(services, nameof(services));
        Ensure.NotNull(configureOptions, nameof(configureOptions));

        services.Configure(configureOptions);

        services
            .AddHttpClient<FritzBox>()
            .ConfigureHttpClient((sp, x) =>
            {
                var options = sp.GetRequiredService<IOptions<FritzBoxConnection>>().Value;

                x.BaseAddress = options.Url;
            })
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var options = sp.GetRequiredService<IOptions<FritzBoxConnection>>().Value;

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

        services.AddTransient<IFritzBox>(sp =>
        {
            var fritzBox = new FritzBox(sp.GetRequiredService<IHttpClientFactory>().CreateClient("FritzBox"));

            return fritzBox;
        });
    }

    public static void AddFritzBox(this IServiceCollection services)
    {
        Ensure.NotNull(services, nameof(services));

        services.AddNamedHttpClientOptions();

        services.AddSingleton<IFritzBoxConnections, FritzBoxConnections>();

        services.AddTransient<IFritzBoxFactory, FritzBoxFactory>();

        services.AddTransient<IFritzBox>(sp =>
        {
            var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("FritzBox");

            var fritzBox = new FritzBox(client);

            return fritzBox;
        });
    }
}

public interface IFritzBoxConnections
{
    void Add(string name, FritzBoxConnection options);

    FritzBoxConnection Get(string name);
}

public class FritzBoxConnections : IFritzBoxConnections
{
    private readonly IHttpClientSettings _httpClientSettings;

    private readonly IDictionary<string, FritzBoxConnection> _optionList;

    public FritzBoxConnections(IHttpClientSettings httpClientSettings)
    {
        _httpClientSettings = httpClientSettings;
        _optionList = new ConcurrentDictionary<string, FritzBoxConnection>();
    }

    public void Add(string name, FritzBoxConnection options)
    {
        _optionList[name] = options;

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

    public FritzBoxConnection Get(string name)
    {
        return _optionList[name];
    }
}

public class FritzBoxConnection
{
    public Uri Url { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool AllowUntrustedCertificates { get; set; }
}
