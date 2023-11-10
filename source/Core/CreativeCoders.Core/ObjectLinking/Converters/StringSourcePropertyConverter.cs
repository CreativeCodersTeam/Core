namespace CreativeCoders.Core.ObjectLinking.Converters;

public class StringSourcePropertyConverter<T> : IPropertyValueConverter
{
    private readonly StringTargetPropertyConverter<T> _stringTargetPropertyConverter = new();

    public object Convert(object value, object parameter)
    {
        return _stringTargetPropertyConverter.ConvertBack(value, parameter);
    }

    public object ConvertBack(object value, object parameter)
    {
        return _stringTargetPropertyConverter.Convert(value, parameter);
    }
}
