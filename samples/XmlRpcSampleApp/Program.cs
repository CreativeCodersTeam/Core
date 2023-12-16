using System;
using System.Threading.Tasks;

namespace XmlRpcSampleApp;

// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    public static async Task Main()
    {
        await new XmlRpcSample().RunAsync();
            
        Console.WriteLine("Press key to exit...");
        Console.ReadKey();
    }
}