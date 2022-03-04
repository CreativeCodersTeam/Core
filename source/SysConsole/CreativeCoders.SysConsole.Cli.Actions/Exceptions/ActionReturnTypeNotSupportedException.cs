using System;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Exceptions
{
    [PublicAPI]
    public class ActionReturnTypeNotSupportedException : CliActionException
    {
        public ActionReturnTypeNotSupportedException(Type returnType)
            : base($"Action return type '{returnType.Name}' is not supported")
        {
            ReturnType = returnType;
        }

        public Type ReturnType { get; }
    }
}
