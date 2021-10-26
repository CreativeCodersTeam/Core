using System;

namespace CreativeCoders.SysConsole.CliArguments.Options
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionParameterAttribute : Attribute
    {
        public OptionParameterAttribute(char shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
        }

        public char ShortName { get; }

        public string LongName { get; }

        public object? DefaultValue { get; set; }

        public bool IsRequired { get; set; }
    }
}
