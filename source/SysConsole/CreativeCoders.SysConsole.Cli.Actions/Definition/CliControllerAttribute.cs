using System;

namespace CreativeCoders.SysConsole.Cli.Actions.Definition
{
    public sealed class CliControllerAttribute : Attribute
    {
        public CliControllerAttribute() : this(string.Empty)
        {
            
        }

        public CliControllerAttribute(string route)
        {
            Route = route;
        }

        public string Route { get; }
    }
}
