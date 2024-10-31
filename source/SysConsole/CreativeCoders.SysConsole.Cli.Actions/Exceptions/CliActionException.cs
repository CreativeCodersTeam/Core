using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Exceptions;

[PublicAPI]
[ExcludeFromCodeCoverage]
public class CliActionException : Exception
{
    public CliActionException() { }

    public CliActionException(string? message) : base(message) { }

    public CliActionException(string? message, Exception? innerException) : base(message, innerException) { }
}
