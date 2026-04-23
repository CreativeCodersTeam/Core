using System.Reflection;
using CreativeCoders.Core.Threading;

namespace CreativeCoders.Core.ObjectLinking;

/// <summary>
///     Copies property values between source and target objects, applying an optional
///     <see cref="IPropertyValueConverter"/> and preventing recursive copy cycles.
/// </summary>
public class PropertyValueCopier
{
    private readonly SynchronizedValue<bool> _isInCopyProperty = new SynchronizedValue<bool>(false);

    /// <summary>
    ///     Copies the value of a source property to a target property, optionally applying a converter.
    ///     Recursive calls are suppressed to prevent infinite loops in bidirectional links.
    /// </summary>
    /// <param name="source">The source object to read the property value from.</param>
    /// <param name="sourceProperty">The <see cref="PropertyInfo"/> of the source property.</param>
    /// <param name="target">The target object to write the property value to.</param>
    /// <param name="targetProperty">The <see cref="PropertyInfo"/> of the target property.</param>
    /// <param name="backDirection">
    ///     <see langword="true"/> to use the converter's <see cref="IPropertyValueConverter.ConvertBack"/> method;
    ///     <see langword="false"/> to use <see cref="IPropertyValueConverter.Convert"/>.
    /// </param>
    /// <param name="info">The property link information containing the converter and converter parameter.</param>
    public void CopyPropertyValue(object source, PropertyInfo sourceProperty, object target,
        PropertyInfo targetProperty, bool backDirection, PropertyLinkInfo info)
    {
        if (_isInCopyProperty.Value)
        {
            return;
        }

        _isInCopyProperty.Value = true;

        try
        {
            var propertyValue = sourceProperty.GetValue(source);

            if (info.Converter != null)
            {
                propertyValue = backDirection
                    ? info.Converter.ConvertBack(propertyValue, info.ConverterParameter)
                    : info.Converter.Convert(propertyValue, info.ConverterParameter);

                if (propertyValue == PropertyLink.DoNothing)
                {
                    return;
                }

                if (propertyValue == PropertyLink.Error)
                {
                    return;
                }
            }

            targetProperty.SetValue(target, propertyValue);
        }
        finally
        {
            _isInCopyProperty.Value = false;
        }
    }
}
