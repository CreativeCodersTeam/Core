using System;
using JetBrains.Annotations;

namespace CreativeCoders.Config.Base.Exceptions;

[PublicAPI]
public class ConfigurationFileSourceException : ConfigurationSourceException
{
    public ConfigurationFileSourceException(string fileName, IConfigurationSource configurationSource,
        string message, Exception exception) : base(configurationSource, message, exception)
    {
        FileName = fileName;
    }

    public string FileName { get; }
}