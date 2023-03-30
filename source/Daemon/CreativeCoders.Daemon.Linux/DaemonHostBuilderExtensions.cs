using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CreativeCoders.Daemon.Linux;

[PublicAPI]
public static class DaemonHostBuilderExtensions
{
    /// <summary>
    /// Setup the <see cref="IDaemonHostBuilder"/> for building a systemd daemon service
    /// </summary>
    /// <param name="daemonHostBuilder"></param>
    /// <returns>The same instance of <see cref="IDaemonHostBuilder"/></returns>
    public static IDaemonHostBuilder UseSystemd(this IDaemonHostBuilder daemonHostBuilder)
    {
        return daemonHostBuilder
            .WithInstaller<SystemdServiceInstaller>()
            .ConfigureHostBuilder(x => x.UseSystemd())
            .ConfigureServices(services =>
                services.AddLogging(x => x.AddSystemdConsole()));
    }
}
