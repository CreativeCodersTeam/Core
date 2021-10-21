using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App
{
    public interface IConsoleApp
    {
        Task<int> RunAsync();
    }
}
