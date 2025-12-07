using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Cli.Hosting;

public class DefaultCliHostBuilder : ICliHostBuilder
{
    private readonly IAssemblyCommandScanner _commandScanner;

    private readonly List<Assembly> _scanAssemblies = [];

    private readonly List<Action<IServiceCollection>> _configureServicesActions = [];

    public DefaultCliHostBuilder(IAssemblyCommandScanner commandScanner)
    {
        _commandScanner = Ensure.NotNull(commandScanner);

        var entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly != null)
        {
            ScanAssemblies(entryAssembly);
        }
    }

    public ICliHostBuilder UseContext<TContext>(Action<TContext>? configure = null)
        where TContext : class, ICliCommandContext
    {
        return ConfigureServices(x => x.AddSingleton(sp =>
        {
            var context = sp.CreateInstance<TContext>();

            configure?.Invoke(context);

            return context;
        }));
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

    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddCliHosting();

        _configureServicesActions.ForEach(x => x(services));

        return services.BuildServiceProvider();
    }

    public ICliHost Build()
    {
        var sp = BuildServiceProvider();

        var commandStore = sp.GetRequiredService<ICliCommandStore>();
        commandStore.AddCommands(_commandScanner.Scan(_scanAssemblies));

        return sp.GetRequiredService<ICliHost>();
    }
}
