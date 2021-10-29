using System;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions
{
    [PublicAPI]
    public class OptionCreationFailedException : CliArgumentsException
    {
        public OptionCreationFailedException(Type optionType)
            : base($"Option of type '{optionType.Name}' cannot be created")
        {
            OptionType = optionType;
        }

        public OptionCreationFailedException(Type optionType, Exception innerException)
            : base($"Option of type '{optionType.Name}' cannot be created", innerException)
        {
            OptionType = optionType;
        }

        public Type OptionType { get; }
    }
}
