using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    public class TestProgramMainWithConfiguration : IMain
    {
        private readonly int _returnCode;

        public TestProgramMainWithConfiguration(IConfiguration configuration)
        {
            _returnCode = int.Parse(configuration["ReturnCode"]);
        }

        public Task<int> ExecuteAsync()
        {
            return Task.FromResult(_returnCode);
        }
    }
}
