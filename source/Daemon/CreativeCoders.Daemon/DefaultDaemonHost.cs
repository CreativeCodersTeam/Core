using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon;

internal class DefaultDaemonHost : IDaemonHost
{
    private readonly IHost _host;

    private readonly DaemonHostSetupInfo _daemonHostSetupInfo;

    public DefaultDaemonHost(IHost host, DaemonHostSetupInfo daemonHostSetupInfo)
    {
        _host = host;
        _daemonHostSetupInfo = daemonHostSetupInfo;
    }

    public async Task RunAsync()
    {
        if (_daemonHostSetupInfo.Args?.Contains(_daemonHostSetupInfo.InstallArg) == true)
        {
            //CreateInstaller().Install();
        }

        if (_daemonHostSetupInfo.Args?.Contains(_daemonHostSetupInfo.UninstallArg) == true)
        {
            //CreateInstaller().Uninstall();
        }

        await _host.RunAsync().ConfigureAwait(false);
    }

    private IDaemonInstaller CreateInstaller()
    {
        if (_daemonHostSetupInfo.InstallerType == null)
        {
            throw new InvalidOperationException("No installer type defined");
        }

        if (Activator.CreateInstance(_daemonHostSetupInfo.InstallerType) is not IDaemonInstaller installer)
        {
            throw new InvalidOperationException("Installer could not be created");
        }

        return installer;
    }
}
