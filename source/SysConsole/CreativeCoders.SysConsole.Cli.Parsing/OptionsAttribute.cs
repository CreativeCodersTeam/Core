using System;

namespace CreativeCoders.SysConsole.Cli.Parsing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OptionsAttribute : Attribute
    {
        public bool AllArgsMustMatch { get; set; }
    }
}
