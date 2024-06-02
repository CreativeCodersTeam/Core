using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.SysConsole.Cli.Parsing.OptionProperties.ValueConverters;

public class EnumerableValueConverter : ICliValueConverter
{
    public object? Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
    {
        if (value == null)
        {
            return ConverterAction.DoNothing;
        }

        if (!targetType.IsAssignableTo(typeof(IEnumerable)))
        {
            return ConverterAction.DoNothing;
        }

        var itemType = typeof(object);

        if (targetType.IsConstructedGenericType &&
            targetType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            itemType = targetType.GetGenericArguments()[0];
        }

        var values = value.ToStringSafe().Split(optionAttribute.Separator);

        var result = values.Select(x => System.Convert.ChangeType(x, itemType)).ToArray();

        return itemType == typeof(object)
            ? result
            : result.OfType(itemType);
    }
}
