using System;
using CreativeCoders.Config.Base;
using CreativeCoders.Core;

namespace CreativeCoders.Config.Sources
{
    public class ConfigurationSource<T> : IConfigurationSource<T>
        where T : class, new()
    {
        private readonly Func<T> _getSettingObjectFunc;

        private readonly Func<T> _getDefaultSettingObjectFunc;

        public ConfigurationSource(Func<T> getSettingObjectFunc) : this(getSettingObjectFunc, () => new T()) { }

        public ConfigurationSource(Func<T> getSettingObjectFunc, Func<T> getDefaultSettingObjectFunc)
        {
            Ensure.IsNotNull(getSettingObjectFunc, nameof(getSettingObjectFunc));
            Ensure.IsNotNull(getDefaultSettingObjectFunc, nameof(getDefaultSettingObjectFunc));

            _getSettingObjectFunc = getSettingObjectFunc;
            _getDefaultSettingObjectFunc = getDefaultSettingObjectFunc;
        }

        public virtual object GetSettingObject()
        {
            return _getSettingObjectFunc();
        }

        public virtual object GetDefaultSettingObject()
        {
            return _getDefaultSettingObjectFunc();
        }
    }
}