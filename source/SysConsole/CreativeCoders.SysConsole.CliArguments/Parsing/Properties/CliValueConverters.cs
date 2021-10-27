using System;
using System.Collections.Generic;
using CreativeCoders.SysConsole.CliArguments.Parsing.Properties.ValueConverters;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties
{
    public class CliValueConverters : ICliValueConverter
    {
        private readonly IDictionary<Type, ICliValueConverter> _converters;

        private CliValueConverters()
        {
            _converters = new Dictionary<Type, ICliValueConverter>
                { {typeof(bool), new BooleanValueConverter()} };
        }

        public static ICliValueConverter Default { get; } = new CliValueConverters();

        public object? Convert(object? value, Type targetType)
        {
            return _converters.TryGetValue(targetType, out var converter)
                ? converter.Convert(value, targetType)
                : System.Convert.ChangeType(value, targetType);
        }
    }
}
