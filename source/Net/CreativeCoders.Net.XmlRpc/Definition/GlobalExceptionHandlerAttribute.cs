using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Definition;

[PublicAPI]
[AttributeUsage(AttributeTargets.Interface)]
public class GlobalExceptionHandlerAttribute : Attribute
{
    public GlobalExceptionHandlerAttribute(Type exceptionHandler)
    {
        ExceptionHandler = exceptionHandler;
    }
        
    public Type ExceptionHandler { get; }
}