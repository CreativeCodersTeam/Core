using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.Cli.Actions.Runtime;

namespace CreativeCoders.SysConsole.Cli.Actions
{
    public interface ICliStartup : IStartup
    {
        void Configure(ICliActionRuntimeBuilder runtimeBuilder);
    }
}
