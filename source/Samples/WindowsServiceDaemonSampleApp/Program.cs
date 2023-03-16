using System.Threading.Tasks;
using CreativeCoders.Daemon;
using CreativeCoders.Daemon.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace WindowsServiceDaemonSampleApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var workerConfig = new SampleWorkerConfig
        {
            TestData = "SampleData"
        };

        await DaemonHostBuilder
            .CreateBuilder<SampleDaemonService>()
            .WithArgs(args)
            .ConfigureServices(x => x.AddSingleton(workerConfig))
            .UseWindowsService()
            .Build()
            .RunAsync()
            .ConfigureAwait(false);
    }
}
