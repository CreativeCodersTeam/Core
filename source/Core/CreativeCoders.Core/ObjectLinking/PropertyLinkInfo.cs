using System.Reflection;

namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Contains resolved information about a property link, including source and target objects,
///     their properties, direction, and converter details.
/// </summary>
public class PropertyLinkInfo
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
    ///     Gets or initializes the <see cref="PropertyInfo"/> of the source property.
    /// </summary>
    public PropertyInfo SourceProperty { get; init; }

    /// <summary>
    ///     Gets or initializes the <see cref="PropertyInfo"/> of the target property.
    /// </summary>
    public PropertyInfo TargetProperty { get; init; }

    /// <summary>
    ///     Gets or sets the direction in which values are synchronized.
    /// </summary>
    public LinkDirection Direction { get; set; }

    /// <summary>
    ///     Gets or initializes the converter used to transform values between linked properties.
    /// </summary>
    public IPropertyValueConverter Converter { get; init; }

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
