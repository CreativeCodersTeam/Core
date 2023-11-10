using System;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

[PublicAPI]
public static class CliActionRuntimeBuilderExtensions
{
    public static ICliActionRuntimeBuilder UseRouting(this ICliActionRuntimeBuilder runtimeBuilder)
    {
        return runtimeBuilder.UseMiddleware<CliRoutingMiddleware>();
    }

    public static ICliActionRuntimeBuilder UseExceptionHandling(this ICliActionRuntimeBuilder runtimeBuilder,
        int errorReturnCode = ExceptionHandlerMiddleware.DefaultErrorReturnCode)
    {
        return runtimeBuilder.UseMiddleware<ExceptionHandlerMiddleware>((Action<CliActionContext>) Nop,
            errorReturnCode);

        static void Nop(CliActionContext _) { }
    }

    public static ICliActionRuntimeBuilder UseExceptionHandling(this ICliActionRuntimeBuilder runtimeBuilder,
        Action<CliActionContext> exceptionHandler,
        int errorReturnCode = ExceptionHandlerMiddleware.DefaultErrorReturnCode)
    {
        return runtimeBuilder.UseMiddleware<ExceptionHandlerMiddleware>(exceptionHandler,
            errorReturnCode);
    }
}
