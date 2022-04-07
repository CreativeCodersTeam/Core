using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Exceptions;

///-------------------------------------------------------------------------------------------------
/// <summary>   Exception for signalling ambiguous route errors. </summary>
///
/// <seealso cref="CliActionException"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class AmbiguousRouteException : CliActionException
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the
    ///     CreativeCoders.SysConsole.Cli.Actions.Exceptions.AmbiguousRouteException class.
    /// </summary>
    ///
    /// <param name="args">     The command line arguments. </param>
    /// <param name="routes">   The ambiguous routes found for arguments. </param>
    ///-------------------------------------------------------------------------------------------------
    public AmbiguousRouteException(IEnumerable<string> args, IEnumerable<CliActionRoute> routes)
    {
        Arguments = args;
        Routes = routes;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the command line arguments. </summary>
    ///
    /// <value> The command line arguments. </value>
    ///-------------------------------------------------------------------------------------------------
    public IEnumerable<string> Arguments { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the ambiguous routes found for arguments. </summary>
    ///
    /// <value> The ambiguous routes. </value>
    ///-------------------------------------------------------------------------------------------------
    public IEnumerable<CliActionRoute> Routes { get; }
}
