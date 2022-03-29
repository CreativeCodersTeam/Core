using CreativeCoders.Config.Base;
using CreativeCoders.Core;

namespace CreativeCoders.Config;

public class SettingFactory<T> : ISettingFactory<T>
    where T : class
{
    private readonly IConfiguration _configuration;

    public SettingFactory(IConfiguration configuration)
    {
        Ensure.IsNotNull(configuration, nameof(configuration));

        _configuration = configuration;
    }

    public T Create()
    {
        return _configuration.GetItem<T>();
    }
}