using System;
using System.Threading.Tasks;

namespace NetSampleApp;

public static class Program
{
    public static async Task Main()
    {
        await new HttpClientAuthTest().Run();
            
        await new HttpClientTest().Run();
            
        AvmTest.Run();
            
        Console.WriteLine("Hello World!");
        Console.ReadKey();
    }
}