using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    public interface ICliActionRouter
    {
        CliActionRoute? FindRoute(IList<string> args);

        void AddRoute(CliActionRoute actionRoute);

        IEnumerable<CliActionRoute> ActionRoutes { get; }
    }
}
