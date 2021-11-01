using System;
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

        public CliActionRoute? FindRoute(string[] args)
        {
            return FindRoute(args, x => x.RouteParts.SequenceEqual(args.Take(x.RouteParts.Length)))
                   ?? GetDefaultRoute(args);
        }

        private CliActionRoute? GetDefaultRoute(string[] args)
        {
            return FindRoute(args, x =>
                   {
                       var routeLength = x.RouteParts.Length - 1;
                       return x.RouteParts.Take(routeLength).SequenceEqual(args.Take(routeLength));
                   })
                   ??
                   FindRoute(args, x =>
                   {
                       var routeLength = x.RouteParts.Length - 1;
                       return x.RouteParts.FirstOrDefault() == string.Empty
                              && x.RouteParts.Skip(1).Take(routeLength).SequenceEqual(args.Take(routeLength));
                   })
                   ??
                   FindRoute(args, x => x.RouteParts.Take(2).All(string.IsNullOrEmpty));
        }

        private CliActionRoute? FindRoute(IEnumerable<string> args, Func<CliActionRoute, bool> routeFilter)
        {
            var routes = _actionRoutes
                .Where(routeFilter)
                .ToArray();

            if (routes.Length > 1)
            {
                throw new AmbiguousRouteException(args, routes);
            }

            return routes.FirstOrDefault();
        }

        public void AddRoute(CliActionRoute actionRoute)
        {
            _actionRoutes.Add(actionRoute);
        }

        public IEnumerable<CliActionRoute> ActionRoutes => _actionRoutes;
    }
}
