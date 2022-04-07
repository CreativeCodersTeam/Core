using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Exceptions;

[PublicAPI]
public class ReturnTypeNotSupportedException : ApiException
{
    public ReturnTypeNotSupportedException(MethodInfo method) :
        base($"Method '{method.Name}' has not supported return type '{method.ReturnType.Name}'")
    {
        Method = method;
    }

    public MethodInfo Method { get; }
}
