using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.Daemon;

[PublicAPI]
public interface IDaemonHostBuilder
{
    /// <summary>
    /// Adds command line arguments to the host builder
    /// </summary>
    /// <param name="args"></param>
    /// <returns>The same instance of <see cref="IDaemonHostBuilder"/></returns>
    IDaemonHostBuilder WithArgs(string[] args);

    /// <summary>
    /// Adds services to the container. This can be called multiple times and the results will be additive.
    /// </summary>
    /// <param name="configureServices">The delegate for configuring the <see cref="IServiceCollection"/> that will be used
    /// to construct the <see cref="IServiceProvider"/>.</param>
    /// <returns>The same instance of the <see cref="IDaemonHostBuilder"/> for chaining.</returns>
    IDaemonHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

    /// <summary>
    /// Configures the underlying <see cref="IHostBuilder"/>
    /// </summary>
    /// <param name="configureHostBuilder">The delegate for configuring <see cref="IHostBuilder"/></param>
    /// <returns>The same instance of the <see cref="IDaemonHostBuilder"/> for chaining.</returns>
    IDaemonHostBuilder ConfigureHostBuilder(Action<IHostBuilder> configureHostBuilder);

    /// <summary>
    /// Use installer with type <typeparamref name="TInstaller"/>
    /// </summary>
    /// <typeparam name="TInstaller">Type of the installer used</typeparam>
    /// <returns>The same instance of the <see cref="IDaemonHostBuilder"/> for chaining.</returns>
    IDaemonHostBuilder WithInstaller<TInstaller>()
        where TInstaller : class, IDaemonInstaller;

    /// <summary>
    /// Use installer with type <typeparamref name="TInstaller"/> and set arguments for install and
    /// uninstall via console arguments
    /// </summary>
    /// <param name="installArg"></param>
    /// <param name="uninstallArg"></param>
    /// <typeparam name="TInstaller"></typeparam>
    /// <returns></returns>
    IDaemonHostBuilder WithInstaller<TInstaller>(string installArg, string uninstallArg)
        where TInstaller : class, IDaemonInstaller;

    /// <summary>
    /// Use definition file <param name="fileName"/> if needed for install or uninstall
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    IDaemonHostBuilder WithDefinitionFile(string fileName);

    /// <summary>
    /// Build the daemon host for running the host
    /// </summary>
    /// <returns></returns>
    IDaemonHost Build();
}
