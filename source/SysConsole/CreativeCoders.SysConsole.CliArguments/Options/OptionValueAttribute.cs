using System;

namespace CreativeCoders.SysConsole.CliArguments.Options
{
    public class OptionValueAttribute : Attribute
    {
        public OptionValueAttribute(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }
}
