using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public interface ICliActionRuntimeBuilder
    {
        ICliActionRuntimeBuilder UseMiddleware<TMiddleware>(params object[]? arguments)
            where TMiddleware : CliActionMiddlewareBase;

        ICliActionRuntime Build();
    }
}
