using JetBrains.Annotations;

namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Defines methods for converting property values between source and target objects in a property link.
/// </summary>
[PublicAPI]
public interface IPropertyValueConverter
{
    /// <summary>
    ///     Converts a source property value to the target property type.
    /// </summary>
    /// <param name="value">The source property value to convert.</param>
    /// <param name="parameter">An optional conversion parameter.</param>
    /// <returns>The converted value for the target property.</returns>
    object Convert(object value, object parameter);

    /// <summary>
    ///     Converts a target property value back to the source property type.
    /// </summary>
    /// <param name="value">The target property value to convert back.</param>
    /// <param name="parameter">An optional conversion parameter.</param>
    /// <returns>The converted value for the source property.</returns>
    object ConvertBack(object value, object parameter);
}
