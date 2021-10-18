using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    public class TestErrorProgramMain : IMain
    {
        public const int ReturnCode = -3456;

        public Task<int> ExecuteAsync()
        {
            return Task.FromResult(ReturnCode);
        }
    }
}
