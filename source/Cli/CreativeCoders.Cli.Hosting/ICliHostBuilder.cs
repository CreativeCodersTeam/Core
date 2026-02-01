using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Help;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Cli.Hosting;

/// <summary>
/// Defines a builder for configuring and creating an instance of <see cref="ICliHost"/>.
/// Provides methods to set up services, command context, assembly scanning, validation, and help functionality
/// for a Command Line Interface (CLI) application.
/// </summary>
[PublicAPI]
public interface ICliHostBuilder
{
    /// <summary>
    /// Configures a custom command context for the CLI application.
    /// </summary>
    /// <typeparam name="TContext">The type of the context to be used. Must implement <see cref="ICliCommandContext"/>.</typeparam>
    /// <param name="configure">
    /// An optional action to configure the context instance. The action receives the service provider
    /// and the instance of <typeparamref name="TContext"/> as parameters.
    /// </param>
    /// <returns>The same <see cref="ICliHostBuilder"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the context is already configured.</exception>
    ICliHostBuilder UseContext<TContext>(Action<IServiceProvider, TContext>? configure = null)
        where TContext : class, ICliCommandContext;

    /// <summary>
    /// Configures additional services for the CLI application.
    /// </summary>
    /// <param name="configureServices">
    /// An action that receives an <see cref="IServiceCollection"/> to add or modify services for the application.
    /// </param>
    /// <returns>The same <see cref="ICliHostBuilder"/> instance, enabling method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configureServices"/> is null.</exception>
    ICliHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

    /// <summary>
    /// Registers specified assemblies for command scanning within the CLI application.
    /// </summary>
    /// <param name="assemblies">
    /// An array of <see cref="Assembly"/> instances to be scanned for commands.
    /// </param>
    /// <returns>The same <see cref="ICliHostBuilder"/> instance.</returns>
    ICliHostBuilder ScanAssemblies(params Assembly[] assemblies);

    /// <summary>
    /// Enables help functionality for the CLI application by specifying the type of help commands to be supported.
    /// </summary>
    /// <param name="commandKinds">
    /// Defines the type of help commands that can be used within the application.
    /// This can be a command-specific help, argument-specific help, or both, as specified by the values in <see cref="HelpCommandKind"/>.
    /// </param>
    /// <returns>The same <see cref="ICliHostBuilder"/> instance to allow for method chaining.</returns>
    ICliHostBuilder EnableHelp(params HelpCommandKind[] commandKinds);

    /// <summary>
    /// Enables or disables command options validation.
    /// </summary>
    /// <param name="useValidation">A boolean value indicating whether validation should be enabled. Default is true.</param>
    /// <returns>The same <see cref="ICliHostBuilder"/> instance.</returns>
    ICliHostBuilder UseValidation(bool useValidation = true);

    /// <summary>
    /// Specifies whether the entry assembly should be excluded from the scanning process
    /// when configuring the CLI application.
    /// </summary>
    /// <param name="skipScanEntryAssembly">
    /// A boolean value indicating whether to skip scanning the entry assembly. Defaults to <c>true</c>.
    /// If set to <c>false</c>, the entry assembly will be included in the assembly scanning process.
    /// </param>
    /// <returns>The same <see cref="ICliHostBuilder"/> instance.</returns>
    ICliHostBuilder SkipScanEntryAssembly(bool skipScanEntryAssembly = true);

    ICliHostBuilder RegisterPreProcessor<T>(Action<T>? configure = null) where T : class, ICliPreProcessor;

    ICliHostBuilder RegisterPostProcessor<T>(Action<T>? configure = null) where T : class, ICliPostProcessor;

    /// <summary>
    /// Builds and creates an instance of <see cref="ICliHost"/> configured through the current builder.
    /// </summary>
    /// <returns>An instance of <see cref="ICliHost"/> that represents the configured Command Line Interface (CLI) application.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the build process encounters an invalid state or configuration.</exception>
    ICliHost Build();
}
