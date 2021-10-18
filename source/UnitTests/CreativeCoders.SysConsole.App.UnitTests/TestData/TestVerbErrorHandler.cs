using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using CreativeCoders.SysConsole.App.Verbs;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    public class TestVerbErrorHandler : IVerbParserErrorHandler
    {
        public const int ReturnCode = -2345;

        public Task<int> HandleErrorsAsync(IEnumerable<Error> errors)
        {
            return Task.FromResult(ReturnCode);
        }
    }
}
