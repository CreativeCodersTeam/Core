using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Exceptions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Exception for signalling no action for the context was set. </summary>
    ///
    /// <seealso cref="CliActionException"/>
    ///-------------------------------------------------------------------------------------------------
    [PublicAPI]
    public class NoActionException : CliActionException
    {
        
    }
}
