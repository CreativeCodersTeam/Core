using System.Collections.Generic;
using CreativeCoders.Config.Base;
using CreativeCoders.Core;

namespace CreativeCoders.Config
{
    public class Settings<T> : ISettings<T>
        where T : class
    {
        private readonly ISettingsFactory<T> _settingsFactory;

        private IEnumerable<T> _values;

        public Settings(ISettingsFactory<T> settingsFactory)
        {
            Ensure.IsNotNull(settingsFactory, nameof(settingsFactory));

            _settingsFactory = settingsFactory;
        }

        public IEnumerable<T> Values => _values ?? (_values = _settingsFactory.Create());
    }
}