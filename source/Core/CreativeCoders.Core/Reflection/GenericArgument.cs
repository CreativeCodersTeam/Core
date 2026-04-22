using System;

namespace CreativeCoders.Core.Reflection;

/// <summary>
/// Represents a named generic type argument used when invoking generic methods dynamically.
/// </summary>
public class GenericArgument
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericArgument"/> class.
    /// </summary>
    /// <param name="name">The name of the generic type parameter.</param>
    /// <param name="type">The concrete type to substitute for the generic parameter.</param>
    public GenericArgument(string name, Type type)
    {
        Ensure.IsNotNullOrWhitespace(name, nameof(name));
        Ensure.IsNotNull(type, nameof(type));

        Name = name;
        Type = type;
    }

    /// <summary>
    /// Gets the name of the generic type parameter.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the concrete type to substitute for the generic parameter.
    /// </summary>
    public Type Type { get; }
}
