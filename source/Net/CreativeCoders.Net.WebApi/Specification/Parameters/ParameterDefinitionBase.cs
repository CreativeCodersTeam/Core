using System;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Specification.Parameters;

[PublicAPI]
public abstract class ParameterDefinitionBase<TValue>
{
    private readonly Func<object, TValue> _transformValue;

    protected ParameterDefinitionBase(ParameterInfo parameterInfo, Func<object, TValue> transformValue)
    {
        _transformValue = transformValue;
        ParameterInfo = parameterInfo;
    }

    public ParameterInfo ParameterInfo { get; }

    public TValue GetValue(object[] arguments)
    {
        if (ParameterInfo.Position >= arguments.Length)
        {
            throw new ArgumentOutOfRangeException();
        }

        var value = arguments[ParameterInfo.Position];

        return _transformValue(value);
    }
}