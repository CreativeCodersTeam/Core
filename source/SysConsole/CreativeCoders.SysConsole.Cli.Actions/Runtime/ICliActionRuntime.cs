using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime;

/// <summary>   Interface for CLI action runtime. </summary>
public interface ICliActionRuntime
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Asynchronously executes the the action for the arguments. </summary>
    ///
    /// <param name="args"> The command line arguments. </param>
    ///
    /// <returns>   Returns the exit code for the program. </returns>
    ///-------------------------------------------------------------------------------------------------
    Task<int> ExecuteAsync(string[] args);
}