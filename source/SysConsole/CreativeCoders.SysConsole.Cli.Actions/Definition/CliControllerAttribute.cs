using System;

namespace CreativeCoders.SysConsole.Cli.Actions.Definition
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Attribute for CLI controller. This class cannot be inherited. </summary>
    ///
    /// <seealso cref="Attribute"/>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CliControllerAttribute : Attribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the
        ///     CreativeCoders.SysConsole.Cli.Actions.Definition.CliControllerAttribute class with a
        ///     default route.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public CliControllerAttribute() : this(string.Empty)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the
        ///     CreativeCoders.SysConsole.Cli.Actions.Definition.CliControllerAttribute class with a
        ///     route.
        /// </summary>
        ///
        /// <param name="route">    The route. </param>
        ///-------------------------------------------------------------------------------------------------
        public CliControllerAttribute(string route)
        {
            Route = route;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the route for the controller. </summary>
        ///
        /// <value> The route. </value>
        ///-------------------------------------------------------------------------------------------------
        public string Route { get; }
    }
}
