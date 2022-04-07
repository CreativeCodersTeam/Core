using System;
using System.Reflection;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Exceptions;

[PublicAPI]
public class WrongReturnTypeException : Exception
{
    public WrongReturnTypeException(MethodInfo methodInfo)
        : base(
            $"XmlRpc method '{methodInfo.Name}' has a wrong return type '{methodInfo.ReturnType.FullName}'. Allowed are Task and Task<>.")
    {
        ReturnType = methodInfo.ReturnType;
        Method = methodInfo;
    }

    public MethodInfo Method { get; }

    public Type ReturnType { get; }
}
