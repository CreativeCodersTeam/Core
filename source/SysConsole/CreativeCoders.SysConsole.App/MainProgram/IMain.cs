using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.MainProgram;

[PublicAPI]
public interface IMain
{
    Task<int> ExecuteAsync(string[] args);
}
