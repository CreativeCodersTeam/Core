using CreativeCoders.Core;
using CreativeCoders.Options.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Options.Storage.FileSystem;

public static class FileSysOptionsStorageServiceCollectionExtensions
{
    public static void AddFileSystemOptionsStorage<T>(this IServiceCollection services, string directoryPath)
        where T : class
    {
        Ensure.NotNull(services);
        Ensure.IsNotNullOrEmpty(directoryPath);

        services.AddNamedConfigurationOptions<T, FileSystemOptionsStorageProvider<T>>(x =>
            x.DirectoryPath = directoryPath);
    }
}
