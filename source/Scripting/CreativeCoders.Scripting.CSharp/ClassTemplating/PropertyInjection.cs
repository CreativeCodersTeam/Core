using System;

namespace CreativeCoders.Scripting.CSharp.ClassTemplating;

internal class PropertyInjection<T> : IScriptClassInjection
{
    private readonly string _propertyName;

    private readonly Func<T> _getInjectionData;

    public PropertyInjection(string propertyName, Func<T> getInjectionData)
    {
        _propertyName = propertyName;
        _getInjectionData = getInjectionData;
    }

    public void Inject(object scriptObject)
    {
        var property = scriptObject.GetType().GetProperty(_propertyName);
        property?.SetValue(scriptObject, _getInjectionData());
    }
}
