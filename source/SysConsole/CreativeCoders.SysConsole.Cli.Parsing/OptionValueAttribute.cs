using System;

namespace CreativeCoders.SysConsole.Cli.Parsing
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class OptionValueAttribute : OptionBaseAttribute
    {
        public OptionValueAttribute(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }
}
