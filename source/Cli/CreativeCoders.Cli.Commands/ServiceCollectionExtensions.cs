using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Cli.Commands;

/// <summary>
/// Service collection extensions for registering the <c>CreativeCoders.Cli.Commands</c> services.
/// </summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the default <see cref="IConfirmationPrompt"/>, <see cref="IInteractivePrompter"/>,
    /// and open-generic <see cref="IOutputFormatter{TResult}"/> implementations (JSON, Table, Plain).
    /// Existing registrations are preserved (TryAdd semantics for the singletons).
    /// <para>
    /// <see cref="Spectre.Console.IAnsiConsole"/> is required and is expected to be registered by
    /// <c>AddCliHosting()</c> from <c>CreativeCoders.Cli.Hosting</c>.
    /// </para>
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection AddCliCommandsBaseClasses(this IServiceCollection services)
    {
        Ensure.NotNull(services);

        services.TryAddSingleton<IConfirmationPrompt, SpectreConfirmationPrompt>();
        services.TryAddSingleton<IInteractivePrompter, SpectreInteractivePrompter>();

        services.AddTransient(typeof(IOutputFormatter<>), typeof(JsonOutputFormatter<>));
        services.AddTransient(typeof(IOutputFormatter<>), typeof(TableOutputFormatter<>));
        services.AddTransient(typeof(IOutputFormatter<>), typeof(PlainOutputFormatter<>));

        return services;
    }
}
