using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions
{
    [PublicAPI]
    public class CliCommandCreationFailedException : CliArgumentsException
    {
        public CliCommandCreationFailedException(Type commandType, Exception innerException)
            : this($"Command '{commandType.Name}' can not be created", innerException)
        {
        }

        public CliCommandCreationFailedException()
        {
        }

        public CliCommandCreationFailedException(string? message) : base(message)
        {
        }

        public CliCommandCreationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CliCommandCreationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
