using System;
using CreativeCoders.Net.Avm;

namespace NetSampleApp;

public static class AvmTest
{
    public static void Run()
    {
        var args = Environment.GetCommandLineArgs();

        var user = args[1];
        var password = args[2];
            
        var fritzBox = new FritzBox("https://fritz.box", user, password);

        var device = fritzBox.Wlan.GetWlanDeviceInfo("8C:B8:4A:CA:8F:29");
            
        Console.WriteLine($"Ip: {device.IpAddress}");
    }
}