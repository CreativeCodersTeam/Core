using System;
using System.Collections.Generic;
using System.Reflection;
using CreativeCoders.SysConsole.CliArguments.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties
{
    public abstract class OptionPropertyBase
    {
        private readonly OptionBaseAttribute _optionAttribute;

        protected OptionPropertyBase(PropertyInfo propertyInfo, OptionBaseAttribute optionAttribute)
        {
            Info = propertyInfo;
            _optionAttribute = optionAttribute;
        }

        protected bool SetPropertyValue(OptionArgument? optionArgument, object optionObject)
        {
            if (optionArgument == null && _optionAttribute.DefaultValue == null)
            {
                if (_optionAttribute.IsRequired)
                {
                    throw new RequiredArgumentMissingException(Info, _optionAttribute);
                }

                return false;
            }

            var value = optionArgument == null
                ? _optionAttribute.DefaultValue
                : optionArgument.Value;

            var converter = (_optionAttribute.Converter != null
                ? Activator.CreateInstance(_optionAttribute.Converter) as ICliValueConverter
                : CliValueConverters.Default)
                ?? CliValueConverters.Default;

            var propertyValue = converter.Convert(value, Info.PropertyType, _optionAttribute);

            if (propertyValue != ConverterAction.DoNothing)
            {
                Info.SetValue(optionObject, propertyValue);
            }

            return true;
        }

        protected PropertyInfo Info { get; }

        public abstract bool Read(IEnumerable<OptionArgument> optionArguments, object optionObject);
    }
}
