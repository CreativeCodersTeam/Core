using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    public class CliActionRouter : ICliActionRouter
    {
        private readonly IEnumerable<CliActionRoute> _routes;

        public CliActionRouter(IEnumerable<CliActionRoute> routes)
        {
            _routes = routes;
        }

        public CliActionRoute? FindRoute(IEnumerable<string> args)
        {
            var routes = _routes
                .Where(x =>
                    x.RouteParts.SequenceEqual(args.Take(x.RouteParts.Length)))
                .ToArray();

            if (routes.Length > 1)
            {
                throw new AmbiguousRouteException(args, routes);
            }

            return routes.FirstOrDefault();
        }

        public CliActionRoute? GetDefaultRoute()
        {
            throw new System.NotImplementedException();
        }
    }
}
