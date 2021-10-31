using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    public interface ICliActionRouter
    {
        CliActionRoute? FindRoute(IEnumerable<string> args);

        CliActionRoute? GetDefaultRoute();

        void AddRoute(CliActionRoute actionRoute);

        IEnumerable<CliActionRoute> ActionRoutes { get; }
    }
}
