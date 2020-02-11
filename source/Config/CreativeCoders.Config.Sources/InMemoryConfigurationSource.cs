using System;
using CreativeCoders.Core;

namespace CreativeCoders.Config.Sources
{
    public class InMemoryConfigurationSource<T> : ConfigurationSource<T>
        where T : class, new()
    {
        public InMemoryConfigurationSource(T data) : base(() => data)
        {
            Ensure.IsNotNull(data, nameof(data));
        }

        public InMemoryConfigurationSource(T data, Func<T> getDefaultSettingObject) : base(() => data, getDefaultSettingObject)
        {
            Ensure.IsNotNull(data, nameof(data));
        }
    }
}