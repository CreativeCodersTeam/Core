using System;

namespace CreativeCoders.SysConsole.CliArguments.Options
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
