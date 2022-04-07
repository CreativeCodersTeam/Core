using System;

namespace CreativeCoders.Core.Reflection;

public class GenericArgument
{
    public GenericArgument(string name, Type type)
    {
        Ensure.IsNotNullOrWhitespace(name, nameof(name));
        Ensure.IsNotNull(type, nameof(type));

        Name = name;
        Type = type;
    }

    public string Name { get; }

    public Type Type { get; }
}
