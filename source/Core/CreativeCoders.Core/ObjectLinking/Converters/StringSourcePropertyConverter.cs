namespace CreativeCoders.Core.ObjectLinking.Converters;

/// <summary>
///     Converts property values when the source property is a <see cref="string"/> and the target
///     property is of type <typeparamref name="T"/>. Acts as the inverse of
///     <see cref="StringTargetPropertyConverter{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the target property value.</typeparam>
public class StringSourcePropertyConverter<T> : IPropertyValueConverter
{
    private readonly StringTargetPropertyConverter<T> _stringTargetPropertyConverter =
        new StringTargetPropertyConverter<T>();

    /// <inheritdoc/>
    public object Convert(object value, object parameter)
    {
        return _stringTargetPropertyConverter.ConvertBack(value, parameter);
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, object parameter)
    {
        return _stringTargetPropertyConverter.Convert(value, parameter);
    }
}
