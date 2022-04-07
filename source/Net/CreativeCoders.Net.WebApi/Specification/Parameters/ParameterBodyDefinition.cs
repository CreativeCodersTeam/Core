using System;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Specification.Parameters;

[PublicAPI]
public class ParameterBodyDefinition : ParameterDefinitionBase<object>
{
    public ParameterBodyDefinition(ParameterInfo parameterInfo, Type dataFormatterType) : base(parameterInfo,
        value => value)
    {
        DataFormatterType = dataFormatterType;
    }

    public Type DataFormatterType { get; }
}
