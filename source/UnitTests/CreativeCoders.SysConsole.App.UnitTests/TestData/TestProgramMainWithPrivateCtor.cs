using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.MainProgram;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    [PublicAPI]
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