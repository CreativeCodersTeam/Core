using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Core;

/// <summary>
/// Provides extension methods for <see cref="IServiceProvider"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Gets an existing service or creates a new instance of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of service to get or create.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>The resolved or created service instance.</returns>
    public static T GetServiceOrCreateInstance<T>(this IServiceProvider serviceProvider)
    {
        return ActivatorUtilities.GetServiceOrCreateInstance<T>(serviceProvider);
    }

    /// <summary>
    /// Creates a new instance of the specified type using the service provider for dependency injection.
    /// </summary>
    /// <typeparam name="T">The type of service to create.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="parameters">The additional constructor parameters.</param>
    /// <returns>The created instance.</returns>
    public static T CreateInstance<T>(this IServiceProvider serviceProvider, params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance<T>(serviceProvider, parameters);
    }
}
