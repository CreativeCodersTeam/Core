using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Exceptions;

[PublicAPI]
public class IllegalParameterAttributeException : ApiException
{
    public IllegalParameterAttributeException(ParameterInfo parameter, string message) : base(message)
    {
        Parameter = parameter;
    }

    public ParameterInfo Parameter { get; }
}
