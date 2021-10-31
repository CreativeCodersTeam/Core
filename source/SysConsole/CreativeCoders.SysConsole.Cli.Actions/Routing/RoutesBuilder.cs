using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Actions.Definition;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    public class RoutesBuilder : IRoutesBuilder
    {
        private readonly IList<Type> _controllerTypes;

        public RoutesBuilder()
        {
            _controllerTypes = new List<Type>();
        }

        public void AddController(Type controllerType)
        {
            if (!_controllerTypes.Contains(controllerType))
            {
                _controllerTypes.Add(controllerType);
            }
        }

        public void AddControllers(Assembly assembly)
        {
            assembly.GetExportedTypes()
                .Where(x => x.GetCustomAttribute(typeof(CliControllerAttribute)) != null)
                .ForEach(AddController);
        }

        public IEnumerable<CliActionRoute> BuildRoutes()
        {
            return _controllerTypes.SelectMany(CreateRouteForController).ToArray();
        }

        private IEnumerable<CliActionRoute> CreateRouteForController(Type controllerType)
        {
            var actionMethods =
                from method in controllerType.GetMethods()
                let actionAttribute =
                    method.GetCustomAttribute(typeof(CliActionAttribute)) as CliActionAttribute
                where actionAttribute != null
                select new {Attribute = actionAttribute, Method = method};

            return actionMethods.Select(x => CreateRoute(controllerType, x.Method, x.Attribute));
        }

        private CliActionRoute CreateRoute(Type controllerType, MethodInfo actionMethod,
            CliActionAttribute actionAttribute)
        {
            var routeParts = new List<string>();

            if (controllerType.GetCustomAttribute(typeof(CliControllerAttribute)) is CliControllerAttribute
                controllerAttribute)
            {
                routeParts.AddRange(controllerAttribute.Route.Split("/"));
            }

            routeParts.AddRange(actionAttribute.Route.Split("/"));

            return new CliActionRoute(controllerType, actionMethod, routeParts);
        }
    }
}
