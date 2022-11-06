using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Definition;

[PublicAPI]
public class MethodExceptionHandlerArguments
{
    public Exception MethodException { get; set; }

    public bool Handled { get; set; }
}
