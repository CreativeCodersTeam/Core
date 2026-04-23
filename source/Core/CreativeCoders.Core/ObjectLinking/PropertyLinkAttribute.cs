using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Marks a property as linked to a property on a target object, enabling automatic value synchronization.
/// </summary>
[PublicAPI]
[AttributeUsage(AttributeTargets.Property)]
public class PropertyLinkAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PropertyLinkAttribute"/> class.
    /// </summary>
    /// <param name="targetType">The type of the target object containing the linked property.</param>
    /// <param name="targetPropertyName">The name of the property on the target object to link to.</param>
    public PropertyLinkAttribute(Type targetType, string targetPropertyName)
    {
        TargetType = targetType;
        TargetPropertyName = targetPropertyName;
    }

    /// <summary>
    ///     Gets the type of the target object containing the linked property.
    /// </summary>
    public Type TargetType { get; }

    /// <summary>
    ///     Gets the name of the property on the target object to link to.
    /// </summary>
    public string TargetPropertyName { get; }

    /// <summary>
    ///     Gets or sets the direction in which property values are synchronized.
    /// </summary>
    public LinkDirection Direction { get; set; }

    /// <summary>
    ///     Gets or sets the type of the <see cref="IPropertyValueConverter"/> used to convert values between linked properties.
    /// </summary>
    public Type Converter { get; set; }

    /// <summary>
    ///     Gets or sets an optional parameter passed to the <see cref="IPropertyValueConverter"/>.
    /// </summary>
    public object ConverterParameter { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the source property is initialized with the target property value
    ///     when the link is established.
    /// </summary>
    public bool InitWithTargetValue { get; set; }
}
