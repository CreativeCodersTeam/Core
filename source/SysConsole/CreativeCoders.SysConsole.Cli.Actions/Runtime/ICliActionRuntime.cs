using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public interface ICliActionRuntime
    {
        Task<int> ExecuteAsync(string[] args);
    }
}
