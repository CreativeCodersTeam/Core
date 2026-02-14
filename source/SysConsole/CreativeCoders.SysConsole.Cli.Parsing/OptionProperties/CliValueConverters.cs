using System;
using System.Collections;
using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Parsing.OptionProperties.ValueConverters;

namespace CreativeCoders.SysConsole.Cli.Parsing.OptionProperties;

public class CliValueConverters : ICliValueConverter
{
    private readonly Dictionary<Type, ICliValueConverter> _converters;

    private readonly EnumValueConverter _enumConverter = new EnumValueConverter();

    private readonly EnumerableValueConverter _enumerableConverter = new EnumerableValueConverter();

    private CliValueConverters()
    {
        _converters = new Dictionary<Type, ICliValueConverter>
            { { typeof(bool), new BooleanValueConverter() } };
    }

    private object? InternalConvert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
    {
        return _converters.TryGetValue(targetType, out var converter)
            ? converter.Convert(value, targetType, optionAttribute)
            : System.Convert.ChangeType(value, targetType);
    }

    public object? Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
    {
        if (targetType != typeof(string) && targetType.IsAssignableTo(typeof(IEnumerable)))
        {
            return _enumerableConverter.Convert(value, targetType, optionAttribute);
        }

        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return ConvertNullable(value, targetType, optionAttribute);
        }

        if (value == null && targetType.IsValueType)
        {
            if (targetType == typeof(bool))
            {
                return true;
            }

            return Activator.CreateInstance(targetType);
        }

        return targetType.IsEnum
            ? _enumConverter.Convert(value, targetType, optionAttribute)
            : InternalConvert(value, targetType, optionAttribute);
    }

    private object? ConvertNullable(object? value, Type targetType, OptionBaseAttribute optionAttribute)
    {
        var nullableType = targetType.GetGenericArguments()[0];

        if (value == null)
        {
            if (nullableType != typeof(bool))
            {
                return Activator.CreateInstance(targetType);
            }

            var boolValue = Activator.CreateInstance(targetType, (object?)true);

            return boolValue;
        }

        var valueConverted = nullableType.IsEnum
            ? _enumConverter.Convert(value, nullableType, optionAttribute)
            : InternalConvert(value, nullableType, optionAttribute);

        if (valueConverted == ConverterAction.DoNothing)
        {
            return ConverterAction.DoNothing;
        }

        return valueConverted == null
            ? Activator.CreateInstance(targetType)
            : Activator.CreateInstance(targetType, valueConverted);
    }

    public static ICliValueConverter Default { get; } = new CliValueConverters();
}
