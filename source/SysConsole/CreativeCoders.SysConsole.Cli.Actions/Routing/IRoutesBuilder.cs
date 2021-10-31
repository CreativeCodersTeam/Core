using System;
using System.Collections.Generic;
using System.Reflection;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    public interface IRoutesBuilder
    {
        RoutesBuilder AddController(Type controllerType);

        RoutesBuilder AddControllers(Assembly assembly);

        IEnumerable<CliActionRoute> BuildRoutes();
    }
}
