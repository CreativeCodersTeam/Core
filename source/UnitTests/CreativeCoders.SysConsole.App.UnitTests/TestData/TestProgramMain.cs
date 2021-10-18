using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    public class TestProgramMain : IMain
    {
        public const int ReturnCode = 123;

        public Task<int> ExecuteAsync()
        {
            return Task.FromResult(ReturnCode);
        }
    }
}
