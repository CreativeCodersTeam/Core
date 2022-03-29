using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Tr064.Exceptions;

[PublicAPI]
public class EntryNotFoundException : Exception
{
    public EntryNotFoundException(string address, string message) : base(message)
    {
        Address = address;
    }

    public EntryNotFoundException(string address, string message, Exception innerException) : base(message,
        innerException)
    {
        Address = address;
    }

    public string Address { get; }
}
