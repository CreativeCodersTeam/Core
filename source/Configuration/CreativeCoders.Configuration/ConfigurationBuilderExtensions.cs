using CreativeCoders.Core.IO;
using Microsoft.Extensions.Configuration;

namespace CreativeCoders.Configuration;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder TryAddJsonFile(this IConfigurationBuilder builder, string path)
    {
        return FileSys.File.Exists(path)
            ? builder.AddJsonFile(path)
            : builder;
    }
}
