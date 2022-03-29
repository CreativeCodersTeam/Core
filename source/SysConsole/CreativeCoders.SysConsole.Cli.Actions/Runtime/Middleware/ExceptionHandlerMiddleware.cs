using System;
using System.Threading.Tasks;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

[UsedImplicitly]
public class ExceptionHandlerMiddleware : CliActionMiddlewareBase
{
    public const int DefaultErrorReturnCode = -1024;

    private readonly int _errorReturnCode;

    private readonly Action<CliActionContext> _exceptionHandler;

    public ExceptionHandlerMiddleware(Func<CliActionContext, Task> next,
        Action<CliActionContext> exceptionHandler, int errorReturnCode) : base(next)
    {
        _errorReturnCode = errorReturnCode;
        _exceptionHandler = Ensure.NotNull(exceptionHandler, nameof(exceptionHandler));
    }

    public override async Task InvokeAsync(CliActionContext context)
    {
        try
        {
            await Next(context);
        }
        catch (Exception e)
        {
            context.Exception = e;
            context.ReturnCode = _errorReturnCode;

            _exceptionHandler(context);
        }
    }
}
