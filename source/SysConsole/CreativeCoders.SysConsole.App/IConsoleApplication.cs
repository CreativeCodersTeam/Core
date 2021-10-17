using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App
{
    public interface IConsoleApplication
    {
        Task<int> RunAsync();
    }
}