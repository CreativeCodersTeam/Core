using System;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions;

[PublicAPI]
public class CliCommandCreationFailedException : CliArgumentsException
{
    public CliCommandCreationFailedException(Type commandType, Exception innerException)
        : base($"Command '{commandType.Name}' can not be created", innerException)
    {
    }
}