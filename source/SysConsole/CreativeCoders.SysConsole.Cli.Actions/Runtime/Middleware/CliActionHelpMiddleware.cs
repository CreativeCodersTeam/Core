using System;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Help;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

public class CliActionHelpMiddleware : CliActionMiddlewareBase
{
    private readonly ICliActionHelpPrinter _helpPrinter;

    public CliActionHelpMiddleware(Func<CliActionContext, Task> next, ICliActionHelpPrinter helpPrinter) :
        base(next)
    {
        _helpPrinter = helpPrinter;
    }

    public override Task InvokeAsync(CliActionContext context)
    {
        if (context.Arguments.FirstOrDefault() != "help")
        {
            return Next(context);
        }

        _helpPrinter.PrintHelp(context.Arguments.Skip(1));

        context.ReturnCode = 0;

        return Task.CompletedTask;
    }
}
