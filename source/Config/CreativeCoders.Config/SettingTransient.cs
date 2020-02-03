using CreativeCoders.Config.Base;

namespace CreativeCoders.Config
{
    public class SettingTransient<T> : Setting<T>, ISettingTransient<T>
        where T : class
    {
        public SettingTransient(ISettingFactory<T> settingFactory) : base(settingFactory) { }
    }
}