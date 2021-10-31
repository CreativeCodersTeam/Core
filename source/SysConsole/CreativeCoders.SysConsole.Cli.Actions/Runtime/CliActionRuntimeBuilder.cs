using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using CreativeCoders.SysConsole.Cli.Actions.Runtime.Middleware;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public class CliActionRuntimeBuilder : ICliActionRuntimeBuilder
    {
        private readonly IRoutesBuilder _routesBuilder;

        private readonly IServiceProvider _serviceProvider;

        private readonly IList<Type> _middlewareTypes;

        public CliActionRuntimeBuilder(IRoutesBuilder routesBuilder, IServiceProvider serviceProvider)
        {
            _routesBuilder = Ensure.NotNull(routesBuilder, nameof(routesBuilder));
            _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));

            _middlewareTypes = new List<Type>();
        }

        public ICliActionRuntimeBuilder UseMiddleware<TMiddleware>(params object[]? arguments)
            where TMiddleware : CliActionMiddlewareBase
        {
            _middlewareTypes.Add(typeof(TMiddleware));

            return this;
        }

        public ICliActionRuntimeBuilder AddControllers()
        {
            return AddControllers(Assembly.GetCallingAssembly());
        }

        public ICliActionRuntimeBuilder AddController(Type controllerType)
        {
            _routesBuilder.AddController(controllerType);

            return this;
        }

        public ICliActionRuntimeBuilder AddController<TController>()
        {
            return AddController(typeof(TController));
        }

        public ICliActionRuntimeBuilder AddControllers(Assembly assembly)
        {
            _routesBuilder.AddControllers(assembly);

            return this;
        }

        public ICliActionRuntime Build()
        {
            var runtime = new CliActionRuntime();

            runtime.Init(CreateMiddlewarePipeline, _routesBuilder.BuildRoutes());

            return runtime;
        }

        private Func<CliActionContext, Task> CreateMiddlewarePipeline(
            Func<CliActionContext, Task> executeActionAsync)
        {
            CliActionMiddlewareBase? lastMiddleware = null;

            foreach (var middlewareType in _middlewareTypes.Reverse())
            {
                Func<CliActionContext, Task> next = CreateNext(lastMiddleware, executeActionAsync);

                lastMiddleware = CreateMiddleware(middlewareType, next);
            }

            return CreateNext(lastMiddleware, executeActionAsync);
        }

        private static Func<CliActionContext, Task> CreateNext(CliActionMiddlewareBase? middleware,
            Func<CliActionContext, Task> executeActionAsync)
        {
            Func<CliActionContext, Task> next = middleware != null
                ? middleware.InvokeAsync
                : executeActionAsync;

            return next;
        }

        private CliActionMiddlewareBase CreateMiddleware(Type middlewareType,
            Func<CliActionContext, Task> next)
        {
            var middleware = middlewareType.CreateInstance<CliActionMiddlewareBase>(_serviceProvider, next);

            if (middleware == null)
            {
                throw new InvalidOperationException();
            }

            return middleware;
        }
    }
}
