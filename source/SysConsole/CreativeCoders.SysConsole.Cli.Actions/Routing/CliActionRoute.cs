using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    public class CliActionRoute
    {
        public CliActionRoute(Type controllerType, MethodInfo? actionMethod,
            IEnumerable<string> routeParts)
        {
            ControllerType = Ensure.NotNull(controllerType, nameof(controllerType));
            ActionMethod = Ensure.NotNull(actionMethod, nameof(actionMethod));
            RouteParts = Ensure.NotNull(routeParts, nameof(routeParts)).ToArray();
        }

        public string[] RouteParts { get; }

        public Type ControllerType { get; }

        public MethodInfo? ActionMethod { get; }
    }
}
