using System.Collections.Concurrent;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.DynamicCode.Proxying;

[PublicAPI]
public class InterceptorWithPropertiesBase<T> : InterceptorBase<T>
    where T : class
{
    private readonly ConcurrentDictionary<PropertyInfo, object?> _propertyValues = new();

    protected override void SetProperty(PropertyInfo propertyInfo, object? value)
    {
        _propertyValues[propertyInfo] = value;
    }

    protected override object? GetProperty(PropertyInfo propertyInfo)
    {
        if (_propertyValues.TryGetValue(propertyInfo, out var propertyValue))
        {
            return propertyValue;
        }

        propertyValue = base.GetProperty(propertyInfo);
        _propertyValues[propertyInfo] = propertyValue;

        return propertyValue;
    }
}
