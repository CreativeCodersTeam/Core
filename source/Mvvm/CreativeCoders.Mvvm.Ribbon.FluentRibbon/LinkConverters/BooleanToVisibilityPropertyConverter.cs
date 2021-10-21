using System.Windows;
using CreativeCoders.Core.ObjectLinking;

namespace CreativeCoders.Mvvm.Ribbon.FluentRibbon.LinkConverters
{
    public class BooleanToVisibilityPropertyConverter : IPropertyValueConverter
    {
        public object Convert(object value, object parameter)
        {
            if (value is not bool isVisible)
            {
                return PropertyLink.DoNothing;
            }

            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, object parameter)
        {
            if (value is not Visibility visibility)
            {
                return PropertyLink.DoNothing;
            }

            return visibility == Visibility.Visible;
        }
    }
}
