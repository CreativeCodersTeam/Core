using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class ConsoleException : Exception
{
    public ConsoleException(int returnCode)
    {
        ReturnCode = returnCode;
    }

    public ConsoleException(int returnCode, string message) : base($"{message}. Return code = {returnCode}")
    {
        ReturnCode = returnCode;
    }

    public ConsoleException() { }

    public ConsoleException(string? message) : base(message) { }

    public ConsoleException(string? message, Exception? innerException) : base(message, innerException) { }

    protected ConsoleException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public int ReturnCode { get; }
}
