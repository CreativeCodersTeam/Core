using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CreativeCoders.Daemon.Windows;

public static class DaemonHostBuilderExtensions
{
    public static IDaemonHostBuilder UseWindowsService(this IDaemonHostBuilder daemonHostBuilder)
    {
        return daemonHostBuilder
            .WithInstaller<WindowsServiceInstaller>()
            .ConfigureHostBuilder(x => x.UseWindowsService())
            .ConfigureServices(services =>
                services.AddLogging(x => x.AddEventLog()));
    }
}
