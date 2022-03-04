using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.MainProgram;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    [PublicAPI]
    public class TestProgramMain : IMain
    {
        public const int ReturnCode = 123;

        public Task<int> ExecuteAsync(string[] args)
        {
            return Task.FromResult(ReturnCode);
        }
    }
}
