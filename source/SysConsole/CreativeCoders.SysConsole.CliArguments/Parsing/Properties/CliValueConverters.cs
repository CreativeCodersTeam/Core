using System;
using System.Collections.Generic;
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

        public object? Convert(object? value, Type targetType)
        {
            if (targetType.IsEnum)
            {
                var targetValue = _enumConverter.Convert(value, targetType);

                if (targetValue != ConverterAction.DoNothing)
                {
                    return targetValue;
                }
            }
            
            return _converters.TryGetValue(targetType, out var converter)
                ? converter.Convert(value, targetType)
                : System.Convert.ChangeType(value, targetType);
        }
    }
}
