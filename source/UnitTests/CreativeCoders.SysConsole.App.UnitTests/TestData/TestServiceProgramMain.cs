using System.Threading.Tasks;
using CreativeCoders.SysConsole.App.MainProgram;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData;

[UsedImplicitly]
public class TestServiceProgramMain : IMain
{
    private readonly int _returnCode;

    public TestServiceProgramMain(ITestService testService)
    {
        TextValue = testService.TextValue;
        _returnCode = testService.ReturnCode;
    }

    public Task<int> ExecuteAsync(string[] args)
    {
        return Task.FromResult(_returnCode);
    }

    public static string? TextValue { get; private set; }
}