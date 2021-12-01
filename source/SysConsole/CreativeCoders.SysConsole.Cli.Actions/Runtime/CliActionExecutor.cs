using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.Cli.Actions.Exceptions;
using CreativeCoders.SysConsole.Cli.Parsing;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    internal class CliActionExecutor : ICliActionExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public CliActionExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
        }

        public async Task ExecuteAsync(CliActionContext context)
        {
            if (context.ActionRoute?.ControllerType == null || context.ActionRoute?.ActionMethod == null)
            {
                throw new NoActionException();
            }

            await ExecuteActionRouteAsync(context);
        }

        private async Task ExecuteActionRouteAsync(CliActionContext context)
        {
            var controller = context.ActionRoute!.ControllerType.CreateInstance<object>(_serviceProvider);

            var actionArguments = CreateActionArguments(context);

            var result = context.ActionRoute.ActionMethod.Invoke(controller, actionArguments);

            var isVoid = context.ActionRoute.ActionMethod.ReturnType == typeof(void);

            if (result == null && !isVoid)
            {
                throw new ActionReturnValueNullException();
            }

            if (isVoid)
            {
                return;
            }

            var resultType = result!.GetType();

            if (resultType == typeof(Task<CliActionResult>))
            {
                var actionResult = await (Task<CliActionResult>) result;

                context.ReturnCode = actionResult.ReturnCode;

                return;
            }

            if (resultType == typeof(Task<int>))
            {
                var returnCode = await (Task<int>) result;

                context.ReturnCode = returnCode;

                return;
            }

            if (result is Task taskResult)
            {
                await taskResult;

                return;
            }

            if (resultType == typeof(int))
            {
                context.ReturnCode = (int) result;

                return;
            }

            if (resultType != typeof(CliActionResult))
            {
                throw new ActionReturnTypeNotSupportedException(resultType);
            }
                
            context.ReturnCode = ((CliActionResult)result).ReturnCode;
        }

        private static object[] CreateActionArguments(CliActionContext context)
        {
            var parameters = context.ActionRoute!.ActionMethod.GetParameters();

            switch (parameters.Length)
            {
                case 0:
                    return Array.Empty<object>();
                case > 1:
                    throw new TargetParameterCountException("Action argument count must be 0 or 1");
                default:
                {
                    var option = new OptionParser(parameters.First().ParameterType).Parse(context.Arguments.ToArray());

                    return new[] { option };
                }
            }
        }
    }
}
