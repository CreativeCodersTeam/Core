using System.Reflection;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Exceptions
{
    public class RequiredArgumentMissingException : CliArgumentsException
    {
        public RequiredArgumentMissingException(PropertyInfo missingProperty,
            OptionValueAttribute valueAttribute)
        {
            MissingProperty = missingProperty;
            ValueAttribute = valueAttribute;
        }

        public RequiredArgumentMissingException(PropertyInfo missingProperty,
            OptionParameterAttribute parameterAttribute)
        {
            MissingProperty = missingProperty;
            ParameterAttribute = parameterAttribute;
        }

        public PropertyInfo MissingProperty { get; }

        public OptionParameterAttribute? ParameterAttribute { get; }

        public OptionValueAttribute? ValueAttribute { get; }
    }
}
