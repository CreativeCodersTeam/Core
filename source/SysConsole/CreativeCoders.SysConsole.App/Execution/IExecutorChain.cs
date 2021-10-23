using System.Collections.Generic;

namespace CreativeCoders.SysConsole.App.Execution
{
    public interface IExecutorChain
    {
        IEnumerable<IExecutor> GetExecutors();
    }
}
