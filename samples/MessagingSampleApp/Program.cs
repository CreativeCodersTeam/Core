using System;
using System.Threading.Tasks;

namespace MessagingSampleApp;

internal static class Program
{
    internal static async Task Main()
    {
        await new TestMediator().RunAsync();
        await TestMessageQueue.RunAsync();
            
        Console.WriteLine("Press key to exit...");
        Console.ReadKey();
    }
}
