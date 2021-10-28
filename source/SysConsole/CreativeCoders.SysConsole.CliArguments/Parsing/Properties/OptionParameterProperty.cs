﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties
{
    public class OptionParameterProperty : OptionPropertyBase
    {
        private readonly OptionParameterAttribute _optionParameterAttribute;

        public OptionParameterProperty(PropertyInfo propertyInfo, OptionParameterAttribute optionParameterAttribute)
            : base(propertyInfo)
        {
            _optionParameterAttribute = optionParameterAttribute;
        }

        public override bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject)
        {
            var optionArgument = optionArguments
                .FirstOrDefault(x =>
                    x.Kind == OptionArgumentKind.ShortName
                    && x.OptionName == _optionParameterAttribute.ShortName.ToString() ||
                    x.Kind == OptionArgumentKind.LongName && x.OptionName == _optionParameterAttribute.LongName);

            if (optionArgument == null && _optionParameterAttribute.DefaultValue == null)
            {
                if (_optionParameterAttribute.IsRequired)
                {
                    throw new RequiredArgumentMissingException(Info, _optionParameterAttribute);
                }

                return false;
            }

            var value = optionArgument == null
                ? _optionParameterAttribute.DefaultValue
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
