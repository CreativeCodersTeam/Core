using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Core.Collections;
using CreativeCoders.Net.Avm;
using Microsoft.Extensions.DependencyInjection;

namespace NetSampleApp;

public static class AvmTest
{
    public static async Task Run()
    {
        var args = Environment.GetCommandLineArgs();

        var user = args[1];
        var password = args[2];

        var services = new ServiceCollection();

        services
            .AddHttpClient("FritzBox")
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();

                handler.Credentials = new NetworkCredential(user, password);

                handler.ServerCertificateCustomValidationCallback =
                    (message, certificate2, arg3, arg4) => true;

                return handler;
            }
            );

        var sp = services.BuildServiceProvider();
            
        var fritzBox = new FritzBox(sp.GetRequiredService<IHttpClientFactory>(), "https://fritz.box", user, password);

        var hostEntries = await fritzBox
            .Hosts
            .GetAllHostEntriesAsync()
            .ConfigureAwait(false);

        hostEntries.ForEach(x => Console.WriteLine($"{x.HostName}: {x.IpAddress}"));

        //var device = fritzBox.Wlan.GetWlanDeviceInfo("8C:B8:4A:CA:8F:29");

        //Console.WriteLine($"Ip: {device.IpAddress}");
    }
}
