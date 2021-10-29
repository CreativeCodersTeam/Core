using System;
using System.Collections.Generic;
using CreativeCoders.SysConsole.CliArguments.Options;
using CreativeCoders.SysConsole.CliArguments.Parsing.Properties.ValueConverters;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties
{
    public class CliValueConverters : ICliValueConverter
    {
        private readonly IDictionary<Type, ICliValueConverter> _converters;

        private readonly ICliValueConverter _enumConverter;

        private CliValueConverters()
        {
            _enumConverter = new EnumValueConverter();

            _converters = new Dictionary<Type, ICliValueConverter>
                { {typeof(bool), new BooleanValueConverter()} };
        }

        public static ICliValueConverter Default { get; } = new CliValueConverters();

        public object? Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
        {
            return targetType.IsEnum
                ? _enumConverter.Convert(value, targetType, optionAttribute)
                : InternalConvert(value, targetType, optionAttribute);
        }

        private object? InternalConvert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
        {
            return _converters.TryGetValue(targetType, out var converter)
                            ? converter.Convert(value, targetType, optionAttribute)
                            : System.Convert.ChangeType(value, targetType);
        }
    }
}
