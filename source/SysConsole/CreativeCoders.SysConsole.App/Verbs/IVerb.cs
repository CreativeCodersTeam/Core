using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App.Verbs
{
    public interface IVerb
    {
        Task<int> ExecuteAsync();
    }
}