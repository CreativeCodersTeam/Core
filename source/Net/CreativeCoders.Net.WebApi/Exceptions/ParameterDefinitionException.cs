using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Exceptions;

[PublicAPI]
public class ParameterDefinitionException : ApiException
{
    public ParameterDefinitionException(MethodInfo methodInfo, ParameterInfo parameterInfo) : base(
        $"Method '{methodInfo}' has multiple body definitions")
    {
        Method = methodInfo;
        ParameterInfo = parameterInfo;
    }

    public MethodInfo Method { get; }

    public ParameterInfo ParameterInfo { get; }
}