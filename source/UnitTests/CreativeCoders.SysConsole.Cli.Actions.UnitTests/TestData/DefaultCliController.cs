using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.Cli.Actions.Definition;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    [CliController]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
    public class DefaultCliController
    {
        [CliAction]
        public Task<CliActionResult> DoDefaultAsync()
        {
            return Task.FromResult(new CliActionResult());
        }

        [CliAction("command")]
        public Task<CliActionResult> DoCommandAsync()
        {
            return Task.FromResult(new CliActionResult());
        }
    }
}
