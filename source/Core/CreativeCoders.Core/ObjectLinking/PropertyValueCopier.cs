using System.Reflection;
using CreativeCoders.Core.Threading;

namespace CreativeCoders.Core.ObjectLinking;

public class PropertyValueCopier
{
    private readonly SynchronizedValue<bool> _isInCopyProperty = new(false);

    public void CopyPropertyValue(object source, PropertyInfo sourceProperty, object target,
        PropertyInfo targetProperty, bool backDirection, PropertyLinkInfo info)
    {
        if (_isInCopyProperty.Value)
        {
            return;
        }

        _isInCopyProperty.Value = true;

        try
        {
            var propertyValue = sourceProperty.GetValue(source);

            if (info.Converter != null)
            {
                propertyValue = backDirection
                    ? info.Converter.ConvertBack(propertyValue, info.ConverterParameter)
                    : info.Converter.Convert(propertyValue, info.ConverterParameter);

                if (propertyValue == PropertyLink.DoNothing)
                {
                    return;
                }

                if (propertyValue == PropertyLink.Error)
                {
                    return;
                }
            }

            targetProperty.SetValue(target, propertyValue);
        }
        finally
        {
            _isInCopyProperty.Value = false;
        }
    }
}
