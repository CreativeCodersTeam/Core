using System;

namespace CreativeCoders.SysConsole.Cli.Actions.Definition;

///-------------------------------------------------------------------------------------------------
/// <summary>   Attribute for CLI action. This class cannot be inherited. </summary>
///
/// <seealso cref="Attribute"/>
///-------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class CliActionAttribute : Attribute
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the
    ///     CreativeCoders.SysConsole.Cli.Actions.Definition.CliActionAttribute class with a default
    ///     route.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public CliActionAttribute() : this(string.Empty) { }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the
    ///     CreativeCoders.SysConsole.Cli.Actions.Definition.CliActionAttribute class with a route.
    /// </summary>
    ///
    /// <param name="route">    The route. </param>
    ///-------------------------------------------------------------------------------------------------
    public CliActionAttribute(string route)
    {
        Route = route;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the route for the action. </summary>
    ///
    /// <value> The route. </value>
    ///-------------------------------------------------------------------------------------------------
    public string Route { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the help text. </summary>
    ///
    /// <value> The help text. </value>
    ///-------------------------------------------------------------------------------------------------
    public string? HelpText { get; set; }
}
