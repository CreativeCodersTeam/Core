using System;

namespace CreativeCoders.Core.ObjectLinking.Converters;

/// <summary>
///     Converts property values when the target property is a <see cref="string"/> and the source
///     property is of type <typeparamref name="T"/>. Conversion to string uses
///     <see cref="object.ToString"/>; conversion back uses <see cref="System.Convert.ChangeType(object, Type)"/>.
/// </summary>
/// <typeparam name="T">The type of the source property value.</typeparam>
public class StringTargetPropertyConverter<T> : IPropertyValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, object parameter)
    {
        return value?.ToString();
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, object parameter)
    {
        if (value is not string text)
        {
            return PropertyLink.DoNothing;
        }

        try
        {
            return System.Convert.ChangeType(text, typeof(T));
        }
        catch (Exception)
        {
            return parameter ?? PropertyLink.Error;
        }
    }
}
