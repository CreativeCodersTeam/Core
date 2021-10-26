using System;

namespace CreativeCoders.SysConsole.CliArguments.Options
{
    public class OptionParameterAttribute : Attribute
    {
        public OptionParameterAttribute(char shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
        }

        public char ShortName { get; }

        public string LongName { get; }
    }
}
