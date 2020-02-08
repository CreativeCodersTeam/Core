using System;
using JetBrains.Annotations;

namespace CreativeCoders.Config.Base.Exceptions
{
    [PublicAPI]
    public class ConfigurationSourceException : Exception
    {
        public ConfigurationSourceException(IConfigurationSource configurationSource, string message, Exception exception)
            : base(message, exception)
        {
            ConfigurationSource = configurationSource;
        }

        public IConfigurationSource ConfigurationSource { get; }
    }
}