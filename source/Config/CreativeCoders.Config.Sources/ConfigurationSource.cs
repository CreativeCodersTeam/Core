using System;
using CreativeCoders.Config.Base;
using CreativeCoders.Core;

namespace CreativeCoders.Config.Sources
{
    public class ConfigurationSource<T> : IConfigurationSource<T>
        where T : class, new()
    {
        private readonly Func<T> _getSettingObject;

        private readonly Func<T> _getDefaultSettingObject;

        public ConfigurationSource(Func<T> getSettingObject) : this(getSettingObject, () => new T()) { }

        public ConfigurationSource(Func<T> getSettingObject, Func<T> getDefaultSettingObject)
        {
            Ensure.IsNotNull(getSettingObject, nameof(getSettingObject));
            Ensure.IsNotNull(getDefaultSettingObject, nameof(getDefaultSettingObject));

            _getSettingObject = getSettingObject;
            _getDefaultSettingObject = getDefaultSettingObject;
        }

        public virtual object GetSettingObject()
        {
            return _getSettingObject();
        }

        public virtual object GetDefaultSettingObject()
        {
            return _getDefaultSettingObject();
        }
    }
}