using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Enums;

namespace CreativeCoders.SysConsole.Cli.Parsing.Properties.ValueConverters
{
    public class EnumValueConverter : ICliValueConverter
    {
        public object Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
        {
            if (!targetType.IsEnum)
            {
                return ConverterAction.DoNothing;
            }

            var allEnumFields = EnumUtils.GetEnumFieldInfos(targetType);

            return targetType.GetCustomAttribute<FlagsAttribute>() == null
                ? ConvertEnum(allEnumFields, value)
                : ConvertEnumFlags(allEnumFields, value, targetType);
        }

        private static object ConvertEnumFlags(IDictionary<Enum, FieldInfo> allEnumFields, object? value,
            Type targetType)
        {
            var values = value.ToStringSafe().Split(',');

            var enumValues = values.Select(x => ConvertEnum(allEnumFields, x))
                .Where(x => x != ConverterAction.DoNothing)
                .Select(x => Enum.ToObject(targetType, x));

            var result = enumValues.Aggregate(0, (current, enumValue) => current | (int) enumValue);

            return Enum.ToObject(targetType, result);
        }

        private static object ConvertEnum(IDictionary<Enum, FieldInfo> allEnumFields, object? value)
        {
            var enumField = allEnumFields.Values.FirstOrDefault(x => x.Name == value?.ToString());

            if (enumField != null && allEnumFields.TryGetKeyByValue(enumField, out var enumValue))
            {
                return enumValue;
            }

            var enumFields = allEnumFields
                .Values
                .Where(x =>
                    x.Name.Equals(value?.ToString(), StringComparison.InvariantCultureIgnoreCase))
                .ToArray();

            if (enumFields.IsSingle() && allEnumFields.TryGetKeyByValue(enumFields.First(), out enumValue))
            {
                return enumValue;
            }

            return ConverterAction.DoNothing;
        }
    }
}
