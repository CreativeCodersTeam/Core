using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.IO.Archives;

public static class ArchivesServiceCollectionExtensions
{
    public static void AddZipSupport(this IServiceCollection services)
    {
        services.AddTransient<IZipArchiveFactory, ZipArchiveFactory>();
    }
}
