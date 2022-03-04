using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public interface ICliActionExecutor
    {
        Task ExecuteAsync(CliActionContext context);
    }
}
