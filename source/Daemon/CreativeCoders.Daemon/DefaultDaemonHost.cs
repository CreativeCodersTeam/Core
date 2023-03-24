using CreativeCoders.Daemon.Definition;
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
            await RunInstallerAsync((definition, installer) => installer.Install(definition))
                .ConfigureAwait(false);

            return;
        }

        if (_daemonHostSetupInfo.Args?.Contains(_daemonHostSetupInfo.UninstallArg) == true)
        {
            await RunInstallerAsync((definition, installer) => installer.Uninstall(definition))
                .ConfigureAwait(false);

            return;
        }

        await _host.RunAsync().ConfigureAwait(false);
    }

    private async Task RunInstallerAsync(Action<DaemonDefinition, IDaemonInstaller> execute)
    {
        if (string.IsNullOrWhiteSpace(_daemonHostSetupInfo.DefinitionFileName))
        {
            throw new InvalidOperationException("No definition file set");
        }

        var definition = await DaemonDefinitionFile.LoadAsync(_daemonHostSetupInfo.DefinitionFileName)
            .ConfigureAwait(false);

        if (definition == null)
        {
            throw new InvalidOperationException("Definition could not be deserialized");
        }

        execute(definition, CreateInstaller());
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
