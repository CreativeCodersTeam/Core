using System;
using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties
{
    public interface ICliValueConverter
    {
        object? Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute);
    }
}
