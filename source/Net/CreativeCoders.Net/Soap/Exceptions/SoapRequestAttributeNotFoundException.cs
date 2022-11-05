using System;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Net.Soap.Exceptions;

public class SoapRequestAttributeNotFoundException : Exception
{
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameterInConstructor")]
    public SoapRequestAttributeNotFoundException(Type requestType) : base(
        $"Object of type '{requestType.Name}' must have a SoapRequestAttribute") { }
}
