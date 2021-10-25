using System;
using System.Runtime.Serialization;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions
{
    public class CliCommandCreationFailedException : CliArgumentsException
    {
        public CliCommandCreationFailedException(Type commandType)
            : this($"Command '{commandType.Name}' can not be created")
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
