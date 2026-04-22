namespace CreativeCoders.Core.ObjectLinking.Converters;

/// <summary>
///     Converts property values when the source property is a <see cref="System.Nullable{T}"/> and the
///     target property is a non-nullable value type <typeparamref name="T"/>. Acts as the inverse of
///     <see cref="NullableTargetPropertyConverter{T}"/>.
/// </summary>
/// <typeparam name="T">The underlying value type of the nullable source property.</typeparam>
public class NullableSourcePropertyConverter<T> : IPropertyValueConverter
    where T : struct
{
    private readonly NullableTargetPropertyConverter<T> _nullableTargetPropertyConverter =
        new NullableTargetPropertyConverter<T>();

    /// <inheritdoc/>
    public object Convert(object value, object parameter)
    {
        return _nullableTargetPropertyConverter.ConvertBack(value, parameter);
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, object parameter)
    {
        return _nullableTargetPropertyConverter.Convert(value, parameter);
    }
}
