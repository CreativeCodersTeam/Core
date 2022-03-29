using System;

namespace CreativeCoders.Core.ObjectLinking.Converters;

public class StringTargetPropertyConverter<T> : IPropertyValueConverter
{
    public object Convert(object value, object parameter)
    {
        return value?.ToString();
    }

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
