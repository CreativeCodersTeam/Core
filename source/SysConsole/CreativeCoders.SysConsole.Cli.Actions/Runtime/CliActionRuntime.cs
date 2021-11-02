using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;
using CreativeCoders.SysConsole.CliArguments.Parsing;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public class CliActionRuntime : ICliActionRuntime
    {
        private readonly IServiceProvider _serviceProvider;

        private Func<CliActionContext, Task>? _middlewarePipeline;

        public CliActionRuntime(IServiceProvider serviceProvider)
        {
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
        }

        public void Init(Func<Func<CliActionContext, Task>, Func<CliActionContext, Task>> createPipeline)
        {
            _middlewarePipeline = createPipeline(ExecuteAction);
        }

        private async Task ExecuteAction(CliActionContext context)
        {
            if (context.ActionRoute?.ControllerType == null || context.ActionRoute?.ActionMethod == null)
            {
                throw new NoActionException();
            }

            await ExecuteActionRoute(context);
        }

        private async Task ExecuteActionRoute(CliActionContext context)
        {
            var controller = context.ActionRoute!.ControllerType.CreateInstance<object>(_serviceProvider);

            var actionArguments = CreateActionArguments(context);

            var result = context.ActionRoute.ActionMethod!.Invoke(controller, actionArguments);

            if (result?.GetType() == typeof(Task<CliActionResult>))
            {
                var actionResult = await (Task<CliActionResult>)result;

                context.ReturnCode = actionResult.ReturnCode;
            }
        }

        private object[] CreateActionArguments(CliActionContext context)
        {
            var parameters = context.ActionRoute!.ActionMethod!.GetParameters();

            switch (parameters.Length)
            {
                case 0:
                    return Array.Empty<object>();
                case > 1:
                    throw new TargetParameterCountException("Action argument count must be 0 or 1");
                default:
                {
                    var option = new OptionParser().Parse(parameters.First().ParameterType, context.Arguments.ToArray());

                    return new[] {option};
                }
            }
        }

        public async Task<int> ExecuteAsync(string[] args)
        {
            if (_middlewarePipeline == null)
            {
                throw new InvalidOperationException("CLI action runtime not initialized");
            }

            var context = new CliActionContext(new CliActionRequest(args));

            await _middlewarePipeline(context);

            return context.ReturnCode;
        }
    }
}
