using System;
using System.Linq;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Enums;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties.ValueConverters
{
    public class EnumValueConverter : ICliValueConverter
    {
        public object? Convert(object? value, Type targetType)
        {
            if (!targetType.IsEnum)
            {
                return ConverterAction.DoNothing;
            }

            var enumFields = EnumUtils.GetEnumFieldInfos(targetType);

            var enumField = enumFields.Values.FirstOrDefault(x => x.Name == value?.ToString());

            if (enumField != null && enumFields.TryGetKeyByValue(enumField, out var enumValue))
            {
                return enumValue;
            }

            enumField = enumFields.Values.SingleOrDefault(x => x.Name.Equals(value?.ToString(), StringComparison.InvariantCultureIgnoreCase));

            if (enumField != null && enumFields.TryGetKeyByValue(enumField, out enumValue))
            {
                return enumValue;
            }

            return ConverterAction.DoNothing;
        }
    }
}
