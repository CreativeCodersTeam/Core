using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App;

/// <summary>   Interface for a console application. </summary>
public interface IConsoleApp
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Set if exceptions during execution should be re-thrown. </summary>
    ///
    /// <param name="reThrow">  True to re throw exception, false otherwise. </param>
    ///
    /// <returns>   The IConsoleApp. </returns>
    ///-------------------------------------------------------------------------------------------------
    IConsoleApp ReThrowExceptions(bool reThrow);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Asynchronously executes console app. </summary>
    ///
    /// <returns>   Returns the return code which should be returned as program exit code. </returns>
    ///-------------------------------------------------------------------------------------------------
    Task<int> RunAsync();
}
