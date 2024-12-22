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

        services.TryAddSingleton<IOptionsStorageProvider<T>>(sp =>
        {
            var source = typeof(FileSystemOptionsStorageProvider<T>)
                .CreateInstance<FileSystemOptionsStorageProvider<T>>(sp);

            Ensure.NotNull(source);

            source.DirectoryPath = directoryPath;

            return source;
        });
    }
}
