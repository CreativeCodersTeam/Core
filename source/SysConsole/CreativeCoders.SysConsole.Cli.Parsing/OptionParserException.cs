using System;
using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Parsing.OptionProperties;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing;

[PublicAPI]
public class OptionParserException(
    OptionPropertyBase optionProperty,
    object option,
    IEnumerable<OptionArgument> optionArguments)
    : Exception($"Reading option property for '{option.GetType().Name}' failed")
{
    public OptionPropertyBase OptionProperty { get; } = optionProperty;

    public object Option { get; } = option;

    public IEnumerable<OptionArgument> OptionArguments { get; } = optionArguments;
}
