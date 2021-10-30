using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.Cli.Actions.Execution
{
    public interface ICliActionExecutor
    {
        Task<int> ExecuteAsync(string[] args);
    }
}
