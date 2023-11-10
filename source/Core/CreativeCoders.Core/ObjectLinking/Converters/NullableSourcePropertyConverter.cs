namespace CreativeCoders.Core.ObjectLinking.Converters;

public class NullableSourcePropertyConverter<T> : IPropertyValueConverter
    where T : struct
{
    private readonly NullableTargetPropertyConverter<T> _nullableTargetPropertyConverter = new();

    public object Convert(object value, object parameter)
    {
        return _nullableTargetPropertyConverter.ConvertBack(value, parameter);
    }

    public object ConvertBack(object value, object parameter)
    {
        return _nullableTargetPropertyConverter.Convert(value, parameter);
    }
}
