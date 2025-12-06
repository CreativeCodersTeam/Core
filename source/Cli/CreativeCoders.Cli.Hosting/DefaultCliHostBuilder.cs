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

    private Assembly[] _scanAsemblies;

    public DefaultCliHostBuilder(IAssemblyCommandScanner commandScanner)
    {
        _commandScanner = commandScanner;
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

        throw new NotImplementedException();
    }

    public ICliHostBuilder ScanAssemblies(params Assembly[] assemblies)
    {
        _scanAsemblies = assemblies;

        return this;
    }

    public ICliHost Build()
    {
        var commands = _commandScanner.Scan(_scanAsemblies);

        var commandStore = new CliCommandStore(commands);

        return new DefaultCliHost(commandStore);
    }
}
