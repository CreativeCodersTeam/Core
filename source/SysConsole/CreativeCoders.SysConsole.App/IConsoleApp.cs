using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App
{
    public interface IConsoleApp
    {
        IConsoleApp ReThrowExceptions(bool reThrow);

        Task<int> RunAsync();
    }
}
