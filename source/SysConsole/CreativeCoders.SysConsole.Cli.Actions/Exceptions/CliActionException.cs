using System;
using System.Runtime.Serialization;

namespace CreativeCoders.SysConsole.Cli.Actions.Exceptions
{
    public class CliActionException : Exception
    {
        public CliActionException()
        {
        }

        public CliActionException(string? message) : base(message)
        {
        }

        public CliActionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CliActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
