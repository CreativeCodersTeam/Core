using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions;

public abstract class CliArgumentsException : Exception
{
    protected CliArgumentsException() { }

    protected CliArgumentsException(string? message) : base(message) { }

    protected CliArgumentsException(string? message, Exception? innerException) : base(message,
        innerException) { }
}
