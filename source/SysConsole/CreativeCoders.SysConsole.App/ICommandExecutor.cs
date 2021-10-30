using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App
{
    public interface ICommandExecutor
    {
        Task<int> ExecuteAsync(string[] args);
    }
}
