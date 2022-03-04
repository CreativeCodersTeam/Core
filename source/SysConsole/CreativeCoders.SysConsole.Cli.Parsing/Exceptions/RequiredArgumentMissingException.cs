using System;
using System.Reflection;

namespace CreativeCoders.SysConsole.Cli.Parsing.Exceptions
{
    public class RequiredArgumentMissingException : Exception
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
