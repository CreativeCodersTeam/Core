using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties
{
    public class ValueOptionProperty : OptionPropertyBase
    {
        private readonly OptionValueAttribute _optionValueAttribute;

        public ValueOptionProperty(PropertyInfo propertyInfo, OptionValueAttribute optionValueAttribute) : base(propertyInfo)
        {
            _optionValueAttribute = optionValueAttribute;
        }

        public override bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject)
        {
            var optionArgument = optionArguments
                .Where(x => x.Kind == OptionArgumentKind.Value)
                .Skip(_optionValueAttribute.Index)
                .FirstOrDefault();

            if (optionArgument == null && _optionValueAttribute.DefaultValue == null)
            {
                if (_optionValueAttribute.IsRequired)
                {
                    throw new RequiredArgumentMissingException(Info, _optionValueAttribute);
                }

                return false;
            }

            var value = optionArgument == null
                ? _optionValueAttribute.DefaultValue
                : optionArgument.Value;

            var propertyValue = CliValueConverters.Default.Convert(value, Info.PropertyType);

            if (propertyValue != ConverterAction.DoNothing)
            {
                Info.SetValue(optionObject, propertyValue);
            }
            

            return true;
        }
    }
}
