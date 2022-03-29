using System.Collections.Generic;
using CreativeCoders.Config.Base;
using CreativeCoders.Core;

namespace CreativeCoders.Config;

public class SettingsFactory<T> : ISettingsFactory<T>
    where T : class
{
    private readonly IConfiguration _configuration;

    public SettingsFactory(IConfiguration configuration)
    {
        Ensure.IsNotNull(configuration, nameof(configuration));

        _configuration = configuration;
    }

    public IEnumerable<T> Create()
    {
        return _configuration.GetItems<T>();
    }
}