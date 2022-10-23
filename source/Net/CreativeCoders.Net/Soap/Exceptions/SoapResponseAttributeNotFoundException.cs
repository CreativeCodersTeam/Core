using System;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Net.Soap.Exceptions;

public class SoapResponseAttributeNotFoundException : Exception
{
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameterInConstructor")]
    public SoapResponseAttributeNotFoundException(Type responseType) : base(
        $"Object of type '{responseType.Name}' must have a SoapResponseAttribute") { }
}
