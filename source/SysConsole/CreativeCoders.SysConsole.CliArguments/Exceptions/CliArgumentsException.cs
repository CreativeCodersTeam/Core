using System;
using System.Runtime.Serialization;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions
{
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
