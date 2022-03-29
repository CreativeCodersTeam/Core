using System.Threading.Tasks;
using CreativeCoders.Daemon.Windows;

namespace WindowsServiceDaemonSampleApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var workerConfig = new SampleWorkerConfig
        {
            TestData = "SampleData"
        };

        await new WindowsServiceDaemon<SampleDaemonService, SampleWorkerConfig>(workerConfig).RunAsync(args);
    }
}