using System;

namespace CreativeCoders.SysConsole.Cli.Parsing.Properties
{
    public interface ICliValueConverter
    {
        object? Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute);
    }
}
