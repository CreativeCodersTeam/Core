using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon;

internal class DefaultDaemonHostBuilder<TDaemonService> : IDaemonHostBuilder
    where TDaemonService : class, IDaemonService
{
    private readonly List<Action<IServiceCollection>> _configureServicesActions;

    private readonly List<Action<IHostBuilder>> _configureHostBuilderActions;

    private readonly DaemonHostSetupInfo _daemonHostSetupInfo;

    public DefaultDaemonHostBuilder()
    {
        _configureServicesActions = new List<Action<IServiceCollection>>();
        _configureHostBuilderActions = new List<Action<IHostBuilder>>();
        _daemonHostSetupInfo = new DaemonHostSetupInfo();
    }

    public IDaemonHostBuilder WithArgs(string[] args)
    {
        _daemonHostSetupInfo.Args = args;

        return this;
    }

    public IDaemonHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    {
        _configureServicesActions.Add(configureServices);

        return this;
    }

    public IDaemonHostBuilder ConfigureHostBuilder(Action<IHostBuilder> configureHostBuilder)
    {
        _configureHostBuilderActions.Add(configureHostBuilder);

        return this;
    }

    public IDaemonHostBuilder WithInstaller<TInstaller>() where TInstaller : class, IDaemonInstaller
    {
        _daemonHostSetupInfo.InstallerType = typeof(TInstaller);

        return this;
    }

    public IDaemonHostBuilder WithInstaller<TInstaller>(string installArg, string uninstallArg)
        where TInstaller : class, IDaemonInstaller
    {
        _daemonHostSetupInfo.InstallArg = installArg;
        _daemonHostSetupInfo.UninstallArg = uninstallArg;

        return WithInstaller<TInstaller>();
    }

    public IDaemonHostBuilder WithDefinitionFile(string fileName)
    {
        _daemonHostSetupInfo.DefinitionFileName = fileName;

        return this;
    }

    public IDaemonHost Build()
    {
        var builder = _daemonHostSetupInfo.Args != null
            ? Host.CreateDefaultBuilder(_daemonHostSetupInfo.Args)
            : Host.CreateDefaultBuilder();

        _configureHostBuilderActions
            .ForEach(configureHostBuilder => configureHostBuilder(builder));

        builder.ConfigureServices((_, services) =>
        {
            services.AddHostedService<DaemonWorker>();

            services.TryAddSingleton<IDaemonService, TDaemonService>();

            _configureServicesActions
                .ForEach(configureServices => configureServices(services));
        });

        return new DefaultDaemonHost(builder.Build(), _daemonHostSetupInfo);
    }
}
