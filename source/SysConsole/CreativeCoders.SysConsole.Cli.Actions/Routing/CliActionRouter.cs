using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing;

internal class CliActionRouter : ICliActionRouter
{
    private readonly List<CliActionRoute> _actionRoutes = [];

    private CliActionRoute? GetDefaultRoute(IList<string> args)
    {
        return FindRoute(args, x =>
               {
                   var routeLength = x.RouteParts.Length - 1;
                   return x.RouteParts.Take(routeLength).SequenceEqual(args.Take(routeLength));
               }, x => x.RouteParts.Length - 1)
               ??
               FindRoute(args, x =>
               {
                   var routeLength = x.RouteParts.Length - 1;
                   return x.RouteParts.FirstOrDefault() == string.Empty
                          && x.RouteParts.Skip(1).Take(routeLength).SequenceEqual(args.Take(routeLength));
               }, x => x.RouteParts.Length - 1)
               ??
               FindRoute(args, x => x.RouteParts.Take(2).All(string.IsNullOrEmpty), _ => 0);
    }

    private CliActionRoute? FindRoute(IList<string> args, Func<CliActionRoute, bool> routeFilter,
        Func<CliActionRoute, int> getRouteCount)
    {
        var routes = _actionRoutes
            .Where(routeFilter)
            .Distinct(x => x.ActionMethod)
            .ToArray();

        if (routes.Length > 1 && routes.Select(x => x.RouteParts.Length).Distinct().Count() == 1)
        {
            throw new AmbiguousRouteException(args, routes);
        }

        var route = routes.MaxBy(x => x.RouteParts.Length);

        if (route == null)
        {
            return null;
        }

        var routeCount = getRouteCount(route);

        while (routeCount > 0 && args.Count > 0)
        {
            args.RemoveAt(0);

            routeCount--;
        }

        return route;
    }

    public CliActionRoute? FindRoute(IList<string> args)
    {
        return FindRoute(args, x =>
                   x.RouteParts.SequenceEqual(args.Take(x.RouteParts.Length)), x => x.RouteParts.Length)
               ?? GetDefaultRoute(args);
    }

    public void AddRoute(CliActionRoute actionRoute)
    {
        _actionRoutes.Add(actionRoute);
    }

    public IEnumerable<CliActionRoute> ActionRoutes => _actionRoutes;
}
