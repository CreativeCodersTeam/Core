using System;
using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime;

/// <summary>   A CLI action context. </summary>
[PublicAPI]
public class CliActionContext
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the
    ///     CreativeCoders.SysConsole.Cli.Actions.Runtime.CliActionContext class.
    /// </summary>
    ///
    /// <param name="request">  The request which is handled by the runtime. </param>
    ///-------------------------------------------------------------------------------------------------
    public CliActionContext(CliActionRequest request)
    {
        Request = request;

        Arguments = request.Arguments;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the request. </summary>
    ///
    /// <value> The request. </value>
    ///-------------------------------------------------------------------------------------------------
    public CliActionRequest Request { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the action route which should be used for execution. </summary>
    ///
    /// <value> The action route. </value>
    ///-------------------------------------------------------------------------------------------------
    public CliActionRoute? ActionRoute { get; set; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets or sets the command line arguments for the next middleware. </summary>
    ///
    /// <value> The arguments. </value>
    ///-------------------------------------------------------------------------------------------------
    public IList<string> Arguments { get; set; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Gets or sets the return code which should be returned as program exit code.
    /// </summary>
    ///
    /// <value> The return code. </value>
    ///-------------------------------------------------------------------------------------------------
    public int ReturnCode { get; set; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Gets or sets the exception for exception handling in the execution pipeline.
    /// </summary>
    ///
    /// <value> The exception. </value>
    ///-------------------------------------------------------------------------------------------------
    public Exception? Exception { get; set; }
}
