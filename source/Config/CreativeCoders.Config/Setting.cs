using CreativeCoders.Config.Base;
using CreativeCoders.Core;

namespace CreativeCoders.Config;

public class Setting<T> : ISetting<T>
    where T : class
{
    private readonly ISettingFactory<T> _settingFactory;

    private T _value;

    public Setting(ISettingFactory<T> settingFactory)
    {
        Ensure.IsNotNull(settingFactory, nameof(settingFactory));

        _settingFactory = settingFactory;
    }

    public T Value => _value ??= _settingFactory.Create();
}