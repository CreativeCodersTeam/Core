using System;

namespace CreativeCoders.SysConsole.Cli.Actions.Definition
{
    public class CliActionAttribute : Attribute
    {
        public CliActionAttribute()
        {
            
        }

        public CliActionAttribute(string route)
        {
            Route = route;
        }

        public string Route { get; }
    }
}
