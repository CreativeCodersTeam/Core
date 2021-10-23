using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App.MainProgram;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    public class TestProgramMainWithInvalidCtorArgs : IMain
    {
        public TestProgramMainWithInvalidCtorArgs(string text)
        {
            Ensure.NotNull(text, nameof(text));
        }

        public Task<int> ExecuteAsync(string[] args)
        {
            return Task.FromResult(0);
        }
    }

    public class TestProgramMainWithPrivateCtor : IMain
    {
        private TestProgramMainWithPrivateCtor()
        {
            
        }

        public Task<int> ExecuteAsync(string[] args)
        {
            return Task.FromResult(0);
        }
    }
}
