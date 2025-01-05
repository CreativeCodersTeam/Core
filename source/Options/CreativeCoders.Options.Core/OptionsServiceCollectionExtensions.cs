using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Options.Core;

public static class OptionsServiceCollectionExtensions
{
    public static void AddNamedConfigurationOptions<T>(this IServiceCollection services)
        where T : class
    {
        Ensure.NotNull(services);

        if (services.Any(x => x.ImplementationType == typeof(NamedConfigurationOptions<T>)))
        {
            return;
        }

        services.ConfigureOptions<NamedConfigurationOptions<T>>();
    }

    public static void AddNamedConfigurationOptions<T, TStorageProvider>(this IServiceCollection services,
        Action<TStorageProvider>? configureSource = null)
        where T : class
        where TStorageProvider : class, IOptionsStorageProvider<T>
    {
        Ensure.NotNull(services);

        services.AddNamedConfigurationOptions<T>();

        services.TryAddSingleton<IOptionsStorageProvider<T>>(sp =>
        {
            var source = typeof(TStorageProvider).CreateInstance<TStorageProvider>(sp);

            Ensure.NotNull(source);

            configureSource?.Invoke(source);

            return source;
        });
    }
}
