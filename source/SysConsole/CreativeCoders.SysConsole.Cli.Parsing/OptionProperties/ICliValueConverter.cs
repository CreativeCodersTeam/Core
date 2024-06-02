using System;

namespace CreativeCoders.SysConsole.Cli.Parsing.OptionProperties;

public interface ICliValueConverter
{
    object? Convert(object? value, Type targetType, OptionBaseAttribute optionAttribute);
}
