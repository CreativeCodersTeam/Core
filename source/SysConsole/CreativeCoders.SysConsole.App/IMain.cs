using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App
{
    public interface IMain
    {
        Task<int> ExecuteAsync();
    }
}