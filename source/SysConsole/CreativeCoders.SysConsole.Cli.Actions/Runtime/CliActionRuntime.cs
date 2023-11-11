using System;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime;

internal class CliActionRuntime : ICliActionRuntime
{
    private readonly ICliActionExecutor _actionExecutor;

    private Func<CliActionContext, Task>? _middlewarePipeline;

    public CliActionRuntime(ICliActionExecutor actionExecutor)
    {
        _actionExecutor = Ensure.NotNull(actionExecutor, nameof(actionExecutor));
    }

    public void Init(Func<Func<CliActionContext, Task>, Func<CliActionContext, Task>> createPipeline)
    {
        _middlewarePipeline = createPipeline(_actionExecutor.ExecuteAsync);
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (_middlewarePipeline == null)
        {
            throw new InvalidOperationException("CLI action runtime not initialized");
        }

        var context = new CliActionContext(new CliActionRequest(args));

        await _middlewarePipeline(context).ConfigureAwait(false);

        return context.ReturnCode;
    }
}
