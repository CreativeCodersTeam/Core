using System.Collections.Generic;
using System.Linq;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    public class CliActionRouter : ICliActionRouter
    {
        private readonly IList<CliActionRoute> _actionRoutes;

        public CliActionRouter()
        {
            _actionRoutes = new List<CliActionRoute>();
        }

        public CliActionRoute? FindRoute(IEnumerable<string> args)
        {
            var routes = _actionRoutes
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

        public void AddRoute(CliActionRoute actionRoute)
        {
            _actionRoutes.Add(actionRoute);
        }

        public IEnumerable<CliActionRoute> ActionRoutes => _actionRoutes;
    }
}
