using System;
using System.Globalization;
using System.Windows.Data;
using CreativeCoders.Core.Enums;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf.Converters;

[PublicAPI]
[ValueConversion(typeof(Enum), typeof(string))]
public class EnumTextConverter : IValueConverter
{
    private readonly EnumStringConverter _converter;

    public EnumTextConverter()
    {
        _converter = new EnumStringConverter();
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Enum enumValue)
        {
            return Binding.DoNothing;
        }

        var text = _converter.Convert(enumValue);
        return text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
