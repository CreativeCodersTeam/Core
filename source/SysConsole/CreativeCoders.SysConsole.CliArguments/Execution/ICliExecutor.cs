using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.CliArguments.Execution;

public interface ICliExecutor
{
    Task<int> ExecuteAsync(string[] args);

    int DefaultErrorReturnCode { get; }
}
