namespace CreativeCoders.Core.Enums;

/// <summary>
/// Defines a contract for attributes that provide a custom string representation for enum values.
/// </summary>
public interface IEnumStringAttribute
{
    /// <summary>
    /// Gets the custom string representation for the enum value.
    /// </summary>
    string Text { get; }
}
