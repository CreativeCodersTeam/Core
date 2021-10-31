﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    public interface IRoutesBuilder
    {
        void AddController(Type controllerType);

        void AddControllers(Assembly assembly);

        IEnumerable<CliActionRoute> BuildRoutes();
    }
}
