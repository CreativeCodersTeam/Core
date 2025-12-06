using System.Reflection;
using CreativeCoders.Cli.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Cli.Hosting;

public interface ICliHostBuilder
{
    ICliHostBuilder UseContext<TContext>(Action<TContext>? configure = null)
        where TContext : class, ICliCommandContext;

    ICliHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

    ICliHostBuilder ScanAssemblies(params Assembly[] assemblies);

    ICliHost Build();
}
