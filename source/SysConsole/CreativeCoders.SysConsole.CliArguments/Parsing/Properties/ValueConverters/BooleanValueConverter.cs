using System;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties.ValueConverters
{
    public class BooleanValueConverter : ICliValueConverter
    {
        public object Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            return bool.TryParse(value.ToString(), out var boolValue) && boolValue;
        }
    }
}
