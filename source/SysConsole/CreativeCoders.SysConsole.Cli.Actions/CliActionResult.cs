namespace CreativeCoders.SysConsole.Cli.Actions;

/// <summary>   Encapsulates the result of a CLI action. </summary>
public class CliActionResult
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.SysConsole.Cli.Actions.CliActionResult
    ///     class.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public CliActionResult()
    {
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.SysConsole.Cli.Actions.CliActionResult
    ///     class.
    /// </summary>
    ///
    /// <param name="returnCode">   The return code. </param>
    ///-------------------------------------------------------------------------------------------------
    public CliActionResult(int returnCode)
    {
        ReturnCode = returnCode;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the return code which used as program exit code. </summary>
    ///
    /// <value> The return code. </value>
    ///-------------------------------------------------------------------------------------------------
    public int ReturnCode { get; init; }
}