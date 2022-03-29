using System;
using CreativeCoders.Config.Base;

namespace CreativeCoders.Config;

internal class SourceRegistration
{
    public SourceRegistration(Type dataType, IConfigurationSource source)
    {
        DataType = dataType;
        Source = source;
    }

    public IConfigurationSource Source { get; }

    public Type DataType { get; }
}