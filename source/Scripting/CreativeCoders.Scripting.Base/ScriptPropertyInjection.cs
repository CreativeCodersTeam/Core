using System;
using CreativeCoders.Scripting.Base.Exceptions;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.Base;

[PublicAPI]
public class ScriptPropertyInjection<T> : IInjection
{
    private readonly string _propertyName;

    private readonly Func<T> _getPropertyValue;

    private readonly bool _throwExceptionIfPropertyNotExists;

    public ScriptPropertyInjection(string propertyName, Func<T> getPropertyValue,
        bool throwExceptionIfPropertyNotExists)
    {
        _propertyName = propertyName;
        _getPropertyValue = getPropertyValue;
        _throwExceptionIfPropertyNotExists = throwExceptionIfPropertyNotExists;
    }

    public ScriptPropertyInjection(string propertyName, Func<T> getPropertyValue)
        : this(propertyName, getPropertyValue, false) { }

    public void Inject(object scriptObject)
    {
        var propertyInfo = scriptObject.GetType().GetProperty(_propertyName);

        if (propertyInfo == null)
        {
            if (_throwExceptionIfPropertyNotExists)
            {
                throw new InjectionFailedException($"Property '{_propertyName}' not found");
            }

            return;
        }

        propertyInfo.SetValue(scriptObject, _getPropertyValue());
    }
}
