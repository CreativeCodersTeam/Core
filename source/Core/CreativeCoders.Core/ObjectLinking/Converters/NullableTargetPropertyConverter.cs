namespace CreativeCoders.Core.ObjectLinking.Converters;

/// <summary>
///     Converts property values when the target property is a <see cref="System.Nullable{T}"/> and the
///     source property is a non-nullable value type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The underlying value type of the nullable target property.</typeparam>
public class NullableTargetPropertyConverter<T> : IPropertyValueConverter
    where T : struct
{
    /// <inheritdoc/>
    public object Convert(object value, object parameter)
    {
        if (value is not T structValue)
        {
            return null;
        }

        T? nullableValue = structValue;

        return nullableValue;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, object parameter)
    {
        if (value is T nullableValue)
        {
            return nullableValue;
        }

        if (parameter is T defaultValue)
        {
            return defaultValue;
        }

        return default(T);
    }
}
