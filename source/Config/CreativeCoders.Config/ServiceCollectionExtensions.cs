using CreativeCoders.Config.Base;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Config;

public static class ServiceCollectionExtensions
{
    [PublicAPI]
    public static void AddConfigSystem(this IServiceCollection services, IConfiguration configuration)
    {
        Ensure.NotNull(configuration, nameof(configuration));

        services.TryAddSingleton(typeof(ISettingFactory<>), typeof(SettingFactory<>));
        services.TryAddSingleton(typeof(ISettingsFactory<>), typeof(SettingsFactory<>));

        services.TryAddSingleton(typeof(ISetting<>), typeof(Setting<>));
        services.TryAddSingleton(typeof(ISettings<>), typeof(Settings<>));

        services.TryAddScoped(typeof(ISettingScoped<>), typeof(SettingScoped<>));
        services.TryAddScoped(typeof(ISettingsScoped<>), typeof(SettingsScoped<>));

        services.TryAddTransient(typeof(ISettingTransient<>), typeof(SettingTransient<>));
        services.TryAddTransient(typeof(ISettingsTransient<>), typeof(SettingsTransient<>));

        services.TryAddSingleton(configuration);
    }
}
