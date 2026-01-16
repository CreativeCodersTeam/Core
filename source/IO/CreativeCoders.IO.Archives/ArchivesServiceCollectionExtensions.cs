using CreativeCoders.Core.IO;
using CreativeCoders.IO.Archives.Tar;
using CreativeCoders.IO.Archives.Zip;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.IO.Archives;

public static class ArchivesServiceCollectionExtensions
{
    public static IServiceCollection AddArchives(this IServiceCollection services)
    {
        return services
            .AddFileSystem()
            .AddSingleton<ITarArchiveWriterFactory, TarArchiveWriterFactory>()
            .AddSingleton<IZipArchiveWriterFactory, ZipArchiveWriterFactory>()
            .AddSingleton<IArchiveWriterFactory, ArchiveWriterFactory>()
            .AddSingleton<ITarArchiveReaderFactory, TarArchiveReaderFactory>()
            .AddSingleton<IZipArchiveReaderFactory, ZipArchiveReaderFactory>();
    }
}
