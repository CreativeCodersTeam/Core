using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Options.Core;

public static class OptionsServiceCollectionExtensions
{
    public static void AddNamedOptions<T>(this IServiceCollection services)
        where T : class
    {
        Ensure.NotNull(services);

        services.ConfigureOptions<NamedConfigurationOptions<T>>();
    }

    public static void AddNamedOptions<T, TStorageProvider>(this IServiceCollection services,
        Action<TStorageProvider>? configureSource = null)
        where T : class
        where TStorageProvider : class, IOptionsStorageProvider<T>
    {
        Ensure.NotNull(services);

        services.TryAddSingleton<IOptionsStorageProvider<T>>(sp =>
        {
            var source = typeof(TStorageProvider).CreateInstance<TStorageProvider>(sp);

            Ensure.NotNull(source);

            configureSource?.Invoke(source);

            return source;
        });

        services.ConfigureOptions<NamedConfigurationOptions<T>>();
    }
}
