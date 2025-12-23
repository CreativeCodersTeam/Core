using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Help;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Cli.Hosting;

[PublicAPI]
public interface ICliHostBuilder
{
    ICliHostBuilder UseContext<TContext>(Action<IServiceProvider, TContext>? configure = null)
        where TContext : class, ICliCommandContext;

    ICliHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

    ICliHostBuilder ScanAssemblies(params Assembly[] assemblies);

    ICliHostBuilder EnableHelp(HelpCommandKind commandKind);

    /// <summary>
    /// Enables or disables command options validation.
    /// </summary>
    /// <param name="useValidation">A boolean value indicating whether validation should be enabled. Default is true.</param>
    /// <returns>The same <see cref="ICliHostBuilder"/> instance.</returns>
    ICliHostBuilder UseValidation(bool useValidation = true);

    ICliHostBuilder SkipScanEntryAssembly(bool skipScanEntryAssembly = true);

    ICliHost Build();
}
