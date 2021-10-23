using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App.Execution
{
    public interface ICommandExecutor
    {
        Task<int> Execute(string[] args);
    }
}
