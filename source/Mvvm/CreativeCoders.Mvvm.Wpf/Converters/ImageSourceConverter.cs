using System;
using System.Globalization;
using System.Windows.Data;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf.Converters;

[PublicAPI]
public class ImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var imageUrl = value?.ToString();

        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Binding.DoNothing;
        }

        if (!imageUrl.StartsWith("res://", StringComparison.InvariantCultureIgnoreCase))
        {
            return imageUrl;
        }

        var url = imageUrl[6..];
        var asmIndex = url.IndexOf("/", StringComparison.InvariantCultureIgnoreCase);
        if (asmIndex < 0)
        {
            return imageUrl;
        }

        var assemblyName = url[..asmIndex];
        var path = url[asmIndex..];
        return $"pack://application:,,,/{assemblyName};component{path}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
