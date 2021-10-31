using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Actions.Routing;

namespace CreativeCoders.SysConsole.Cli.Actions.Exceptions
{
    public class AmbiguousRouteException : CliActionException
    {
        public AmbiguousRouteException(IEnumerable<string> args, IEnumerable<CliActionRoute> routes)
        {
            Arguments = args;
            Routes = routes;
        }

        public IEnumerable<string> Arguments { get; }

        public IEnumerable<CliActionRoute> Routes { get; }
    }
}
