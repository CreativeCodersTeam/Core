using System;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing;

[PublicAPI]
[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionParameterAttribute : OptionBaseAttribute
{
    public OptionParameterAttribute(char shortName, string longName)
    {
        ShortName = shortName;
        LongName = longName;
    }

    public OptionParameterAttribute(char shortName)
    {
        ShortName = shortName;
    }

    public OptionParameterAttribute(string longName)
    {
        LongName = longName;
    }

    public char? ShortName { get; }

    public string? LongName { get; }
}