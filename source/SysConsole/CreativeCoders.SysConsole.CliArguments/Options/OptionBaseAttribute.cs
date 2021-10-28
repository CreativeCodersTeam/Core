using System;

namespace CreativeCoders.SysConsole.CliArguments.Options
{
    public abstract class OptionBaseAttribute : Attribute
    {
        public object? DefaultValue { get; set; }

        public bool IsRequired { get; set; }
    }
}
