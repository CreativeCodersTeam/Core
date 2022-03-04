using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;

namespace CreativeCoders.SysConsole.Cli.Actions
{
    /// <summary>   Interface for CLI startup. </summary>
    public interface ICliStartup : IStartup
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Configure the runtime. </summary>
        ///
        /// <param name="runtimeBuilder">   The runtime builder. </param>
        ///-------------------------------------------------------------------------------------------------
        void Configure(ICliActionRuntimeBuilder runtimeBuilder);
    }
}
