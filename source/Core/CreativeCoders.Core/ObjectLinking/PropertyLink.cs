namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Provides sentinel values used by <see cref="IPropertyValueConverter"/> implementations to
///     signal special conversion outcomes.
/// </summary>
public static class PropertyLink
{
    /// <summary>
    ///     Gets a sentinel value indicating that the converter determined no property update should occur.
    /// </summary>
    public static object DoNothing { get; } = new object();

    /// <summary>
    ///     Gets a sentinel value indicating that the conversion failed and the property should not be updated.
    /// </summary>
    public static object Error { get; } = new object();
}
