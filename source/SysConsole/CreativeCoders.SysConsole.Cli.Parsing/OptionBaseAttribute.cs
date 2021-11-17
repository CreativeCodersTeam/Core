using System;

namespace CreativeCoders.SysConsole.Cli.Parsing
{
    public abstract class OptionBaseAttribute : Attribute
    {
        public object? DefaultValue { get; init; }

        public bool IsRequired { get; init; }

        public Type? Converter { get; init; }

        public char Separator { get; init; } = ',';

        public string? HelpText { get; set; }

        public string? Name { get; set; }
    }
}
