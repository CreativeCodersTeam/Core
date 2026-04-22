using System;
using System.Reflection;

namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Represents the definition of a property link between a source and target object, including
///     direction, converter, and initialization settings.
/// </summary>
public class PropertyLinkDefinition
{
    /// <summary>
    ///     Gets or initializes the source object of the property link.
    /// </summary>
    public object Source { get; init; }

    /// <summary>
    ///     Gets or initializes the target object of the property link.
    /// </summary>
    public object Target { get; init; }

    /// <summary>
    ///     Gets or initializes the type of the target object.
    /// </summary>
    public Type TargetType { get; init; }

    /// <summary>
    ///     Gets or initializes the <see cref="PropertyInfo"/> of the source property.
    /// </summary>
    public PropertyInfo SourceProperty { get; init; }

    /// <summary>
    ///     Gets or initializes the name of the target property.
    /// </summary>
    public string TargetPropertyName { get; init; }

    /// <summary>
    ///     Gets or initializes the direction in which values are synchronized.
    /// </summary>
    public LinkDirection LinkDirection { get; init; }

    /// <summary>
    ///     Gets or initializes the type of the <see cref="IPropertyValueConverter"/> used for value conversion.
    /// </summary>
    public Type Converter { get; init; }

    /// <summary>
    ///     Gets or initializes an optional parameter passed to the <see cref="IPropertyValueConverter"/>.
    /// </summary>
    public object ConverterParameter { get; init; }

    /// <summary>
    ///     Gets or initializes a value indicating whether the source property is initialized with the
    ///     target property value when the link is established.
    /// </summary>
    public bool InitWithTargetValue { get; init; }
}
