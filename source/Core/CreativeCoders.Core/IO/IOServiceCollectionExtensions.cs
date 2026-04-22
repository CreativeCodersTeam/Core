using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Core.IO;

/// <summary>
/// Provides extension methods for registering IO services with the dependency injection container.
/// </summary>
[ExcludeFromCodeCoverage]
public static class IOServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IFileSystemEx"/> and <see cref="IFileSystem"/> as singleton services
    /// in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection AddFileSystem(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystemEx, FileSystemEx>();
        services.TryAddSingleton<IFileSystem>(sp => sp.GetRequiredService<IFileSystemEx>());

        return services;
    }
}
