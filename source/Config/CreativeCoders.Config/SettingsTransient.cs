using CreativeCoders.Config.Base;

namespace CreativeCoders.Config
{
    public class SettingsTransient<T> : Settings<T>, ISettingsTransient<T>
        where T : class
    {
        public SettingsTransient(ISettingsFactory<T> settingsFactory) : base(settingsFactory) { }
    }
}