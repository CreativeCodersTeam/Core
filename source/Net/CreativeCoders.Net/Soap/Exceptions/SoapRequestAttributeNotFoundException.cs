using System;

namespace CreativeCoders.Net.Soap.Exceptions;

public class SoapRequestAttributeNotFoundException : Exception
{
    // ReSharper disable once SuggestBaseTypeForParameter
    public SoapRequestAttributeNotFoundException(Type requestType) : base(
        $"Object of type '{requestType.Name}' must have a SoapRequestAttribute") { }
}
