using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using CreativeCoders.Core;
using CreativeCoders.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Net.Avm;

public static class FritzBoxServiceCollectionExtensions
{
    public static void AddFritzBox(this IServiceCollection services, Action<FritzBoxOptions> configureOptions)
    {
        Ensure.NotNull(services, nameof(services));
        Ensure.NotNull(configureOptions, nameof(configureOptions));

        services.Configure(configureOptions);

        services
            .AddHttpClient<FritzBox>()
            .ConfigureHttpClient((sp, x) =>
            {
                var options = sp.GetRequiredService<IOptions<FritzBoxOptions>>().Value;

                //x.BaseAddress = options.Url;
            })
            .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<FritzBoxOptions>>().Value;

                    var handler = new HttpClientHandler();

                    handler.Credentials = new NetworkCredential(options.UserName, options.Password);

                    if (options.AllowUntrustedCertificates)
                    {
                        handler.ServerCertificateCustomValidationCallback =
                            (message, certificate2, arg3, arg4) => true;
                    }
                    
                    return handler;
                }
            );

        services.AddTransient<IFritzBox>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<FritzBoxOptions>>().Value;

            var fritzBox = new FritzBox(sp.GetRequiredService<IHttpClientFactory>().CreateClient("FritzBox"), options.Url.ToString());

            return fritzBox;
        });
    }

    public static void AddFritzBox(this IServiceCollection services, string url, string userName, string password)
    {
        Ensure.NotNull(services, nameof(services));

        services.AddNamedHttpClientOptions();


        //services.AddSingleton<IFritzBoxConnections, FritzBoxConnections>();

        //services.AddHttpClient<FritzBox>();

        //services
        //    .AddHttpClient<FritzBox>()
        //    .ConfigureHttpClient((sp, x) =>
        //    {
        //        var options = sp.GetRequiredService<IOptions<FritzBoxOptions>>().Value;

        //        //x.BaseAddress = options.Url;
        //    })
        //    .ConfigurePrimaryHttpMessageHandler(sp =>
        //        {
        //            var options = sp.GetRequiredService<IOptions<FritzBoxOptions>>().Value;

        //            var handler = new HttpClientHandler();

        //            handler.Credentials = new NetworkCredential(options.UserName, options.Password);

        //            if (options.AllowUntrustedCertificates)
        //            {
        //                handler.ServerCertificateCustomValidationCallback =
        //                    (message, certificate2, arg3, arg4) => true;
        //            }

        //            return handler;
        //        }
        //    );



        services.AddTransient<IFritzBox>(sp =>
        {
            var store = sp.GetRequiredService<INamedHttpClientFactoryOptionsStore>();

            store.Add("FritzBox", x =>
            {
                x.HttpMessageHandlerBuilderActions.Add(builder => builder.PrimaryHandler = new HttpClientHandler()
                {
                    Credentials = new NetworkCredential(userName, password),
                    ServerCertificateCustomValidationCallback =
                        (message, certificate2, arg3, arg4) => true
                });
            });

            var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("FritzBox");

            //var options = sp.GetRequiredService<IOptions<FritzBoxOptions>>().Value;

            var fritzBox = new FritzBox(client, url);

            return fritzBox;
        });
    }
}

public interface IFritzBoxConnections
{
    void Add(string name, FritzBoxOptions options);

    FritzBoxOptions Get(string name);
}

public class FritzBoxConnections : IFritzBoxConnections
{
    private readonly IDictionary<string, FritzBoxOptions> _optionList;

    public FritzBoxConnections()
    {
        _optionList = new ConcurrentDictionary<string, FritzBoxOptions>();
    }

    public void Add(string name, FritzBoxOptions options)
    {
        _optionList[name] = options;
    }

    public FritzBoxOptions Get(string name)
    {
        return _optionList[name];
    }
}

public class FritzBoxOptions
{
    public Uri Url { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool AllowUntrustedCertificates { get; set; }
}
