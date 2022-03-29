using System;
using System.Collections;
using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Parsing.Properties.ValueConverters;

namespace CreativeCoders.SysConsole.Cli.Parsing.Properties;

public class CliValueConverters : ICliValueConverter
{
    private readonly IDictionary<Type, ICliValueConverter> _converters;

    private readonly ICliValueConverter _enumConverter;

    private readonly ICliValueConverter _enumerableConverter;

    private CliValueConverters()
    {
        _enumConverter = new EnumValueConverter();

        _enumerableConverter = new EnumerableValueConverter();

        _converters = new Dictionary<Type, ICliValueConverter>
            { {typeof(bool), new BooleanValueConverter()} };
    }

    public static ICliValueConverter Default { get; } = new CliValueConverters();

    public object? Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
    {
        if (targetType != typeof(string) && targetType.IsAssignableTo(typeof(IEnumerable)))
        {
            return _enumerableConverter.Convert(value, targetType, optionAttribute);
        }

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