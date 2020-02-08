using CreativeCoders.Config.Base;

namespace CreativeCoders.Config
{
    public class SettingScoped<T> : Setting<T>, ISettingScoped<T>
        where T : class
    {
        public SettingScoped(ISettingFactory<T> settingFactory) : base(settingFactory) { }
    }
}