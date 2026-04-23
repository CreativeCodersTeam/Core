using System;

namespace CreativeCoders.Core.Enums;

/// <summary>
/// Specifies a custom string representation for an enum field or enum type.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
public class EnumStringValueAttribute : Attribute, IEnumStringAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumStringValueAttribute"/> class.
    /// </summary>
    /// <param name="text">The string representation for the enum value.</param>
    public EnumStringValueAttribute(string text)
    {
        Text = text;
    }

    /// <inheritdoc />
    public string Text { get; }
}
