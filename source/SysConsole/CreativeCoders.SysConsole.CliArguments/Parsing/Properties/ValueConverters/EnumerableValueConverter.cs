using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties.ValueConverters
{
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

            if (targetType.IsConstructedGenericType && targetType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                itemType = targetType.GetGenericArguments().First();
            }

            var values = value.ToStringSafe().Split(optionAttribute.Separator);

            var result = values.Select(x => System.Convert.ChangeType(x, itemType)).ToArray();

            if (itemType == typeof(string))
            {
                return result.OfType<string>().ToArray();
            }

            if (itemType == typeof(int))
            {
                return result.OfType<int>().ToArray();
            }

            if (itemType == typeof(bool))
            {
                return result.OfType<bool>().ToArray();
            }

            if (itemType == typeof(double))
            {
                return result.OfType<double>().ToArray();
            }

            if (itemType == typeof(DateTime))
            {
                return result.OfType<DateTime>().ToArray();
            }

            return result;
        }
    }
}
