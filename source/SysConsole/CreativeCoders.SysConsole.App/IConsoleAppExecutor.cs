using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App
{
    /// <summary>   Interface for console application executor. </summary>
    public interface IConsoleAppExecutor
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Asynchronously executes the console app program. </summary>
        ///
        /// <param name="args"> The command line arguments. </param>
        ///
        /// <returns>   Returns the return code which should be returned as program exit code. </returns>
        ///-------------------------------------------------------------------------------------------------
        Task<int> ExecuteAsync(string[] args);
    }
}
