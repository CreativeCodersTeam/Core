using System;

namespace CreativeCoders.SysConsole.Cli.Parsing;

[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionValueAttribute(int index) : OptionBaseAttribute
{
    public int Index { get; } = index;
}
