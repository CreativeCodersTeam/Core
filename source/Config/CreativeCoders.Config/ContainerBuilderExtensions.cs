using CreativeCoders.Config.Base;
using CreativeCoders.Core;
using CreativeCoders.Di.Building;

namespace CreativeCoders.Config
{
    public static class ContainerBuilderExtensions
    {
        public static void Configure(this IDiContainerBuilder containerBuilder, IConfiguration configuration)
        {
            Ensure.IsNotNull(configuration, nameof(configuration));

            containerBuilder.AddSingleton(typeof(ISettingFactory<>), typeof(SettingFactory<>));
            containerBuilder.AddSingleton(typeof(ISettingsFactory<>), typeof(SettingsFactory<>));

            containerBuilder.AddSingleton(typeof(ISetting<>), typeof(Setting<>));
            containerBuilder.AddSingleton(typeof(ISettings<>), typeof(Settings<>));

            containerBuilder.AddScoped(typeof(ISettingScoped<>), typeof(SettingScoped<>));
            containerBuilder.AddScoped(typeof(ISettingsScoped<>), typeof(SettingsScoped<>));

            containerBuilder.AddTransient(typeof(ISettingTransient<>), typeof(SettingTransient<>));
            containerBuilder.AddTransient(typeof(ISettingsTransient<>), typeof(SettingsTransient<>));

            containerBuilder.AddSingleton(c => configuration);
        }
    }
}