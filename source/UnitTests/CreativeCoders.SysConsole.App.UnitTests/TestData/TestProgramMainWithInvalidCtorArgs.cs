using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App.MainProgram;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    [PublicAPI]
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
}
