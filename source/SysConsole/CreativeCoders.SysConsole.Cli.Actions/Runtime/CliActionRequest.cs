using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime;

/// <summary>   A CLI action request. </summary>
[PublicAPI]
public class CliActionRequest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the
    ///     CreativeCoders.SysConsole.Cli.Actions.Runtime.CliActionRequest class.
    /// </summary>
    ///
    /// <param name="args"> The command line arguments. </param>
    ///-------------------------------------------------------------------------------------------------
    public CliActionRequest(string[] args)
    {
        Arguments = args.ToList();
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the command line arguments. </summary>
    ///
    /// <value> The arguments. </value>
    ///-------------------------------------------------------------------------------------------------
    public IList<string> Arguments { get; set; }
}