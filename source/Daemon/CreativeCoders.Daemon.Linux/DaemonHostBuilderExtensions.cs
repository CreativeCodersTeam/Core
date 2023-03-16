using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CreativeCoders.Daemon.Linux;

public static class DaemonHostBuilderExtensions
{
    public static IDaemonHostBuilder UseSystemd(this IDaemonHostBuilder daemonHostBuilder)
    {
        return daemonHostBuilder
            .WithInstaller<SystemdServiceInstaller>()
            .ConfigureHostBuilder(x => x.UseSystemd())
            .ConfigureServices(services =>
                services.AddLogging(x => x.AddSystemdConsole()));
    }
}
