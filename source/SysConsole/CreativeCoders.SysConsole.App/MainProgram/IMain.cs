using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App.MainProgram
{
    public interface IMain
    {
        Task<int> ExecuteAsync(string[] args);
    }
}
