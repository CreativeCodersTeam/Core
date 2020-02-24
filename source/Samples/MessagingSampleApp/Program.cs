using System;
using System.Threading.Tasks;

namespace MessagingSampleApp
{
    internal static class Program
    {
        internal static async Task Main()
        {
            await new TestMediator().Run();
            
            Console.WriteLine("Press key to exit...");
            Console.ReadKey();
        }
    }
}