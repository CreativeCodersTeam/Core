using System;
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

        services.AddFritzBox();

        var sp = services.BuildServiceProvider();

        var fritzBoxConnections = sp.GetRequiredService<IFritzBoxConnections>();

        fritzBoxConnections.Add("FritzBox2",
            new FritzBoxConnection
            {
                Url = new Uri("https://fritz.box"),
                UserName = user,
                Password = password,
                AllowUntrustedCertificates = true
            });

        var fritzBoxFactory = sp.GetRequiredService<IFritzBoxFactory>();

        var fritzBox = fritzBoxFactory.Create("FritzBox2");

        var hostEntries = await fritzBox
            .Hosts
            .GetAllHostEntriesAsync()
            .ConfigureAwait(false);

        hostEntries.ForEach(x => Console.WriteLine($"{x.HostName}: {x.IpAddress}"));

        //var device = fritzBox.Wlan.GetWlanDeviceInfo("8C:B8:4A:CA:8F:29");

        //Console.WriteLine($"Ip: {device.IpAddress}");
    }
}
