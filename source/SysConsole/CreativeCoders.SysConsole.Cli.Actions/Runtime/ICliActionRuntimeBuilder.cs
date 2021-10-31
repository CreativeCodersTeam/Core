using System;
using System.Reflection;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public interface ICliActionRuntimeBuilder
    {
        ICliActionRuntimeBuilder UseMiddleware<TMiddleware>(params object[]? arguments)
            where TMiddleware : CliActionMiddlewareBase;

        ICliActionRuntimeBuilder AddControllers();

        ICliActionRuntimeBuilder AddController(Type controllerType);

        ICliActionRuntimeBuilder AddController<TController>();

        ICliActionRuntimeBuilder AddControllers(Assembly assembly);

        ICliActionRuntime Build();
    }
}
