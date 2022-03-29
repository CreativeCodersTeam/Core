using CreativeCoders.Config.Base;

namespace CreativeCoders.Config;

public class SettingsScoped<T> : Settings<T>, ISettingsScoped<T>
    where T : class
{
    public SettingsScoped(ISettingsFactory<T> settingsFactory) : base(settingsFactory) { }
}