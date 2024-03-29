﻿namespace CreativeCoders.Core.ObjectLinking.Converters;

public class NullableTargetPropertyConverter<T> : IPropertyValueConverter
    where T : struct
{
    public object Convert(object value, object parameter)
    {
        if (value is not T structValue)
        {
            return null;
        }

        T? nullableValue = structValue;

        return nullableValue;
    }

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
