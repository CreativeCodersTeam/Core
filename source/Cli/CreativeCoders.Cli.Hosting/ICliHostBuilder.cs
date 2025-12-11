using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Help;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Cli.Hosting;

[PublicAPI]
public interface ICliHostBuilder
{
    ICliHostBuilder UseContext<TContext>(Action<TContext>? configure = null)
        where TContext : class, ICliCommandContext;

    ICliHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

    ICliHostBuilder ScanAssemblies(params Assembly[] assemblies);

    ICliHostBuilder EnableHelp(HelpCommandKind commandKind);

    ICliHost Build();
}
