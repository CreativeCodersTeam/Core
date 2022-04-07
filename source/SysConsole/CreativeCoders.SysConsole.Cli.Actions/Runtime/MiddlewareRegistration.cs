using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime;

internal class MiddlewareRegistration
{
    private readonly Type _middlewareType;

    private readonly object[]? _args;

    public MiddlewareRegistration(Type middlewareType, object[]? args)
    {
        _middlewareType = middlewareType;
        _args = args;
    }

    public CliActionMiddlewareBase CreateMiddleware(Func<CliActionContext, Task> next,
        IServiceProvider serviceProvider)
    {
        var args = new List<object>(_args ?? Array.Empty<object>()) {next};

        var middleware =
            _middlewareType.CreateInstance<CliActionMiddlewareBase>(serviceProvider, args.ToArray());

        if (middleware == null)
        {
            throw new InvalidOperationException();
        }

        return middleware;
    }
}
