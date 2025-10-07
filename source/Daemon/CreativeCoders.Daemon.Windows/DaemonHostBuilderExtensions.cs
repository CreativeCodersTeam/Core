using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public static IDaemonHostBuilder UseWindowsService(this IDaemonHostBuilder daemonHostBuilder)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }

        return daemonHostBuilder
            .WithInstaller<WindowsServiceInstaller>()
            .ConfigureHostBuilder(x => x.UseWindowsService())
            .ConfigureServices(services =>
                services.AddLogging(x => x.AddEventLog()));
    }
}
