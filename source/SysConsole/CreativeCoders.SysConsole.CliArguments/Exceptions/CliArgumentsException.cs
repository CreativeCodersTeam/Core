using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions
{
    [PublicAPI]
    public class CliArgumentsException : Exception
    {
        public CliArgumentsException()
        {
        }

        public CliArgumentsException(string? message) : base(message)
        {
        }

        public CliArgumentsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CliArgumentsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
