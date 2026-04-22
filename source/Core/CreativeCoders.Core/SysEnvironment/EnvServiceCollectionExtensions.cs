using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Core.SysEnvironment;

/// <summary>
/// Provides extension methods for registering environment services with the dependency injection container.
/// </summary>
[ExcludeFromCodeCoverage]
[PublicAPI]
public static class EnvServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IEnvironment"/> with the dependency injection container
    /// using <see cref="EnvironmentWrapper"/> as the default implementation.
    /// </summary>
    /// <param name="services">The service collection to register the environment service with.</param>
    public static void AddEnvironment(this IServiceCollection services)
    {
        services.TryAddSingleton<IEnvironment, EnvironmentWrapper>();
    }
}
