using System.Reflection;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions
{
    public class RequiredArgumentMissingException : CliArgumentsException
    {
        private readonly OptionBaseAttribute _optionAttribute;

        public RequiredArgumentMissingException(PropertyInfo missingProperty,
            OptionBaseAttribute optionAttribute)
        {
            MissingProperty = missingProperty;
            _optionAttribute = optionAttribute;
        }

        public PropertyInfo MissingProperty { get; }

        public OptionParameterAttribute? ParameterAttribute => _optionAttribute as OptionParameterAttribute;

        public OptionValueAttribute? ValueAttribute => _optionAttribute as OptionValueAttribute;
    }
}
