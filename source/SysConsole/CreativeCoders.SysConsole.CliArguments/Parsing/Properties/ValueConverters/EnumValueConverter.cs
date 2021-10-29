using System;
using System.Linq;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Enums;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties.ValueConverters
{
    public class EnumValueConverter : ICliValueConverter
    {
        public object? Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
        {
            if (!targetType.IsEnum)
            {
                return ConverterAction.DoNothing;
            }

            var allEnumFields = EnumUtils.GetEnumFieldInfos(targetType);

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
