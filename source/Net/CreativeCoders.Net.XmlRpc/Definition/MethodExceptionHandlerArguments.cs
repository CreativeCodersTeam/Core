using System;

namespace CreativeCoders.Net.XmlRpc.Definition;

public class MethodExceptionHandlerArguments
{
    public Exception MethodException { get; set; }

    public bool Handled { get; set; }
}