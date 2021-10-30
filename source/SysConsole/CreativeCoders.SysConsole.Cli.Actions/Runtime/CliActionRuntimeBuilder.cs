using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public class CliActionRuntimeBuilder : ICliActionRuntimeBuilder
    {
        private readonly IList<Type> _middlewareTypes;

        public CliActionRuntimeBuilder()
        {
            _middlewareTypes = new List<Type>();
        }

        public ICliActionRuntimeBuilder UseMiddleware<TMiddleware>(params object[]? arguments)
            where TMiddleware : CliActionMiddlewareBase
        {
            _middlewareTypes.Add(typeof(TMiddleware));

            return this;
        }

        public ICliActionRuntime Build()
        {
            return new CliActionRuntime(CreateMiddlewarePipeline());
        }

        private Func<CliActionContext, Task> CreateMiddlewarePipeline()
        {
            CliActionMiddlewareBase? lastMiddleware = null;

            foreach (var middlewareType in _middlewareTypes.Reverse())
            {
                Func<CliActionContext, Task> next = CreateNext(lastMiddleware);

                lastMiddleware = CreateMiddleware(middlewareType, next);
            }

            return CreateNext(lastMiddleware);
        }

        private static Func<CliActionContext, Task> CreateNext(CliActionMiddlewareBase? middleware)
        {
            Func<CliActionContext, Task> next = middleware != null
                ? middleware.InvokeAsync
                : x => Task.CompletedTask;

            return next;
        }

        private static CliActionMiddlewareBase CreateMiddleware(Type middlewareType,
            Func<CliActionContext, Task> next)
        {
            var ctorArgs = new object[] { next };

            if (Activator.CreateInstance(middlewareType, ctorArgs) is not CliActionMiddlewareBase middleware)
            {
                throw new InvalidOperationException();
            }

            return middleware;
        }
    }
}
