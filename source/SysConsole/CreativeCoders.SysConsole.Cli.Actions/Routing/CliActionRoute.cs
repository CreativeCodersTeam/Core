using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing;

/// <summary>   A CLI action route. </summary>
public class CliActionRoute
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the
    ///     CreativeCoders.SysConsole.Cli.Actions.Routing.CliActionRoute class.
    /// </summary>
    /// <param name="controllerType">   The type of the controller which declares the action method. </param>
    /// <param name="actionMethod">     The action method. </param>
    /// <param name="routeParts">       The route parts. </param>
    /// -------------------------------------------------------------------------------------------------
    public CliActionRoute(Type controllerType, MethodInfo actionMethod,
        IEnumerable<string> routeParts)
    {
        ControllerType = Ensure.NotNull(controllerType);
        ActionMethod = Ensure.NotNull(actionMethod);
        RouteParts = Ensure.NotNull(routeParts).ToArray();
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>   Gets the parts of the route. </summary>
    /// <value> The route parts. </value>
    /// -------------------------------------------------------------------------------------------------
    public string[] RouteParts { get; }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>   Gets the type of the controller which declares the action method. </summary>
    /// <value> The type of the controller. </value>
    /// -------------------------------------------------------------------------------------------------
    public Type ControllerType { get; }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>   Gets the action method. </summary>
    /// <value> The action method. </value>
    /// -------------------------------------------------------------------------------------------------
    public MethodInfo ActionMethod { get; }
}
