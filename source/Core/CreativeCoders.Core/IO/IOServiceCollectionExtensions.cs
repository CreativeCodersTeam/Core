using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Core.IO;

[ExcludeFromCodeCoverage]
public static class IOServiceCollectionExtensions
{
    public static IServiceCollection AddFileSystem(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystem, FileSystemEx>();
        services.TryAddSingleton<IFileSystemEx, FileSystemEx>();

        return services;
    }
}
