using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CreativeCoders.Daemon.Windows;

public static class DaemonHostBuilderExtensions
{
    /// <summary>
    /// Setup the <see cref="IDaemonHostBuilder"/> for building a Windows service
    /// </summary>
    /// <param name="daemonHostBuilder"></param>
    /// <returns>The same instance of <see cref="IDaemonHostBuilder"/></returns>
    public static IDaemonHostBuilder UseWindowsService(this IDaemonHostBuilder daemonHostBuilder)
    {
        return daemonHostBuilder
            .WithInstaller<WindowsServiceInstaller>()
            .ConfigureHostBuilder(x => x.UseWindowsService())
            .ConfigureServices(services =>
                services.AddLogging(x => x.AddEventLog()));
    }
}
