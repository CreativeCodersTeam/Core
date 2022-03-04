using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Exceptions
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
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
