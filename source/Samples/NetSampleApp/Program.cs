using System;
using System.Threading.Tasks;

namespace NetSampleApp;

public static class Program
{
    public static async Task Main()
    {
        await HttpClientAuthTest.RunAsync();
            
        await HttpClientTest.RunAsync();
            
        await AvmTest.Run();
            
        Console.WriteLine("Hello World!");
        Console.ReadKey();
    }
}
