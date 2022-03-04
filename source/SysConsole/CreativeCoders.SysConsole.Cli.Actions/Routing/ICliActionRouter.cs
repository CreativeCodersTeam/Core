using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Actions.Routing
{
    /// <summary>   Interface for CLI action router. </summary>
    public interface ICliActionRouter
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Searches for the route matching the arguments. </summary>
        ///
        /// <param name="args"> The arguments. </param>
        ///
        /// <returns>   The found route. </returns>
        ///-------------------------------------------------------------------------------------------------
        CliActionRoute? FindRoute(IList<string> args);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a route. </summary>
        ///
        /// <param name="actionRoute">  The action route. </param>
        ///-------------------------------------------------------------------------------------------------
        void AddRoute(CliActionRoute actionRoute);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all action routes. </summary>
        ///
        /// <value> The action routes. </value>
        ///-------------------------------------------------------------------------------------------------
        IEnumerable<CliActionRoute> ActionRoutes { get; }
    }
}
