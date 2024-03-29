﻿using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

[UsedImplicitly]
public class CliRoutingMiddleware : CliActionMiddlewareBase
{
    private readonly ICliActionRouter _router;

    public CliRoutingMiddleware(ICliActionRouter router, Func<CliActionContext, Task> next) : base(next)
    {
        _router = router;
    }

    public override async Task InvokeAsync(CliActionContext context)
    {
        var route = _router.FindRoute(context.Arguments);

        context.ActionRoute = route;

        await Next(context).ConfigureAwait(false);
    }
}
