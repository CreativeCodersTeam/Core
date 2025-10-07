using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using CreativeCoders.Options.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Options.Serializers;

[ExcludeFromCodeCoverage]
[PublicAPI]
public static class OptionsSerializerServiceCollectionExtensions
{
    public static void AddOptionsJsonSerializer<T>(this IServiceCollection services,
        JsonSerializerOptions? jsonSerializerOptions = null)
        where T : class
    {
        if (jsonSerializerOptions != null)
        {
            services.TryAddSingleton<IOptionsStorageDataSerializer<T>>(_ =>
                new JsonDataSerializer<T>(jsonSerializerOptions));
        }
        else
        {
            services.TryAddSingleton<IOptionsStorageDataSerializer<T>, JsonDataSerializer<T>>();
        }
    }

    public static void AddOptionsYamlSerializer<T>(this IServiceCollection services)
        where T : class
    {
        services.TryAddSingleton<IOptionsStorageDataSerializer<T>, YamlDataSerializer<T>>();
    }
}
