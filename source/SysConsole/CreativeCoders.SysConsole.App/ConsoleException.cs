using System;
using System.Diagnostics.CodeAnalysis;
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

    public int ReturnCode { get; }
}
