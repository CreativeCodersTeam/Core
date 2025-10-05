using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.MainProgram;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData;

[PublicAPI]
public class TestProgramMainWithConfiguration(IConfiguration configuration) : IMain
{
    private readonly int _returnCode = int.Parse(configuration["ReturnCode"] ?? "-123456");

    public Task<int> ExecuteAsync(string[] args)
    {
        return Task.FromResult(_returnCode);
    }
}
