using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Cli.Hosting.Commands.Validation;
using CreativeCoders.Cli.Hosting.Help;
using CreativeCoders.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Cli.Hosting;

public class DefaultCliHostBuilder : ICliHostBuilder
{
    private readonly List<Assembly> _scanAssemblies = [];

    private readonly List<Action<IServiceCollection>> _configureServicesActions = [];

    private bool _helpEnabled;

    private HelpCommandKind _helpCommandKind;

    private bool _skipScanEntryAssembly;

    private bool _contextConfigured;

    public ICliHostBuilder UseContext<TContext>(Action<IServiceProvider, TContext>? configure = null)
        where TContext : class, ICliCommandContext
    {
        if (_contextConfigured)
        {
            throw new InvalidOperationException("Context already configured");
        }

        _contextConfigured = true;

        return ConfigureServices(x => x
            .AddSingleton(sp =>
            {
                var context = sp.CreateInstance<TContext>();

                configure?.Invoke(sp, context);

                return context;
            })
            .AddSingleton<ICliCommandContext, TContext>());
    }

    public ICliHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    {
        Ensure.NotNull(configureServices);

        _configureServicesActions.Add(configureServices);

        return this;
    }

    public ICliHostBuilder ScanAssemblies(params Assembly[] assemblies)
    {
        _scanAssemblies.AddRange(assemblies);

        return this;
    }

    public ICliHostBuilder EnableHelp(HelpCommandKind commandKind)
    {
        _helpEnabled = true;
        _helpCommandKind = commandKind;

        return this;
    }

    public ICliHostBuilder SkipScanEntryAssembly(bool skipScanEntryAssembly = true)
    {
        _skipScanEntryAssembly = skipScanEntryAssembly;

        return this;
    }

    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        if (_helpEnabled)
        {
            services.TryAddSingleton<HelpHandlerSettings>(_ => new HelpHandlerSettings
            {
                CommandKind = _helpCommandKind
            });
            services.TryAddSingleton<ICliCommandHelpHandler, CliCommandHelpHandler>();
        }
        else
        {
            services.TryAddSingleton<ICliCommandHelpHandler, DisabledCommandHelpHandler>();
        }

        _configureServicesActions.ForEach(x => x(services));

        services.AddCliHosting();

        return services.BuildServiceProvider();
    }

    private void ScanEntryAssemblyIfNecessary()
    {
        if (_skipScanEntryAssembly)
        {
            return;
        }

        var entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly != null)
        {
            ScanAssemblies(entryAssembly);
        }
    }

    public ICliHost Build()
    {
        ScanEntryAssemblyIfNecessary();

        if (!_contextConfigured)
        {
            UseContext<CliCommandContext>();
        }

        var sp = BuildServiceProvider();

        var commandScanner = sp.GetRequiredService<IAssemblyCommandScanner>();

        var commandStore = sp.GetRequiredService<ICliCommandStore>();

        var assemblyScanResult = commandScanner.ScanForCommands(_scanAssemblies.ToArray());

        commandStore.AddCommands(assemblyScanResult.CommandInfos, assemblyScanResult.GroupAttributes);

        var validator = sp.GetRequiredService<ICliCommandStructureValidator>();
        validator.Validate(commandStore);

        return sp.GetRequiredService<ICliHost>();
    }
}
