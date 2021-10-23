using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App.Execution
{
    public interface IExecutor
    {
        Task<ExecutorResult> TryExecuteAsync(string[] args);
    }
}