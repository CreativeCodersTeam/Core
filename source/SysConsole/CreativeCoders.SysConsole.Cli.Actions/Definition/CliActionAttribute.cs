using System;

namespace CreativeCoders.SysConsole.Cli.Actions.Definition
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class CliActionAttribute : Attribute
    {
        public CliActionAttribute() : this(string.Empty)
        {
            
        }

        public CliActionAttribute(string route)
        {
            Route = route;
        }

        public string Route { get; }
    }
}
