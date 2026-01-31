using System;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing;

[PublicAPI]
[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionParameterAttribute : OptionBaseAttribute
{
    public OptionParameterAttribute(string shortName, string longName)
    {
        ShortName = Ensure.IsNotNullOrWhitespace(shortName);
        LongName = Ensure.IsNotNullOrWhitespace(longName);
    }

    public OptionParameterAttribute(char shortName, string longName)
    {
        ShortName = shortName.ToString();
        LongName = Ensure.IsNotNullOrWhitespace(longName);
    }

    public OptionParameterAttribute(char shortName)
    {
        ShortName = shortName.ToString();
    }

    public OptionParameterAttribute(string longName)
    {
        LongName = Ensure.IsNotNullOrWhitespace(longName);
    }

    public string? ShortName { get; }

    public string? LongName { get; }
}
