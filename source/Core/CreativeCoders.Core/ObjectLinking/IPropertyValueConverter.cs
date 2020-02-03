using JetBrains.Annotations;

namespace CreativeCoders.Core.ObjectLinking
{
    [PublicAPI]
    public interface IPropertyValueConverter
    {
        object Convert(object value, object parameter);

        object ConvertBack(object value, object parameter);
    }
}