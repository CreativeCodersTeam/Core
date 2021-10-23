using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.MainProgram;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    public class TestErrorProgramMain : IMain
    {
        public const int ReturnCode = -3456;

        public Task<int> ExecuteAsync(string[] args)
        {
            return Task.FromResult(ReturnCode);
        }
    }
}
