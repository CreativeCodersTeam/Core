using System;

namespace CreativeCoders.SysConsole.CliArguments.Options
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionValueAttribute : Attribute
    {
        public OptionValueAttribute(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public object? DefaultValue { get; set; }

        public bool IsRequired { get; set; }
    }
}
