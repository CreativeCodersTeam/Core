﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Actions.Definition;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing;

public class RoutesBuilder : IRoutesBuilder
{
    private readonly List<Type> _controllerTypes = [];

    private static IEnumerable<CliActionRoute> CreateRouteForController(Type controllerType)
    {
        var controllerAttributes =
            controllerType
                .GetCustomAttributes<CliControllerAttribute>()
                .ToArray();

        if (controllerAttributes.Length == 0)
        {
            return Array.Empty<CliActionRoute>();
        }

        var actionMethods =
            (from method in controllerType.GetMethods()
                let actionAttributes =
                    method.GetCustomAttributes<CliActionAttribute>().ToArray()
                where actionAttributes.Length > 0
                select new { Attributes = actionAttributes, Method = method })
            .SelectMany(x =>
                x.Attributes.Select(attr => new { Attribute = attr, x.Method }));

        return controllerAttributes
            .SelectMany(attr =>
                actionMethods.SelectMany(x =>
                    CreateRoute(controllerType, attr, x.Method, x.Attribute)));
    }

    private static IEnumerable<CliActionRoute> CreateRoute(Type controllerType,
        CliControllerAttribute controllerAttribute,
        MethodInfo actionMethod,
        CliActionAttribute actionAttribute)
    {
        var routeParts = new List<string>();

        routeParts.AddRange(controllerAttribute.Route.Split("/").Where(x => !string.IsNullOrEmpty(x)));

        routeParts.AddRange(actionAttribute.Route.Split("/").Where(x => !string.IsNullOrEmpty(x)));

        yield return new CliActionRoute(controllerType, actionMethod, routeParts);

        if (actionAttribute.AlternativeRoute.Length > 0)
        {
            yield return new CliActionRoute(controllerType, actionMethod, actionAttribute.AlternativeRoute);
        }
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
            .Where(x => x.GetCustomAttribute<CliControllerAttribute>() != null)
            .ForEach(AddController);
    }

    public IEnumerable<CliActionRoute> BuildRoutes()
    {
        return _controllerTypes.SelectMany(CreateRouteForController).ToArray();
    }
}
