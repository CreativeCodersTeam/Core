using System;

namespace CreativeCoders.Net.Soap.Exceptions;

public class SoapResponseAttributeNotFoundException : Exception
{
    // ReSharper disable once SuggestBaseTypeForParameter
    public SoapResponseAttributeNotFoundException(Type responseType) : base(
        $"Object of type '{responseType.Name}' must have a SoapResponseAttribute") { }
}
