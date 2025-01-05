using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Options.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Options.Storage.FileSystem;

public static class FileSysOptionsStorageServiceCollectionExtensions
{
    public static void AddFileSystemOptionsStorage<T>(this IServiceCollection services, string directoryPath)
        where T : class
    {
        Ensure.NotNull(services);
        Ensure.IsNotNullOrEmpty(directoryPath);

        services.AddNamedConfigurationOptions<T, FileSystemOptionsStorageProvider<T>>(
            x => x.DirectoryPath = directoryPath);
    }
}
