using System.Threading.Tasks;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.Runtime
{
    public class ActionExecutorTestClass
    {
        public const int AsyncActionResultReturnCode = 1234;

        public const int AsyncIntReturnCode = 2345;

        public const int ActionResultReturnCode = 3456;

        public const int IntReturnCode = 4567;

        public object? ExecuteNull()
        {
            return null;
        }

        public Task<CliActionResult> ExecuteWithActionResultAsync()
        {
            return Task.FromResult(new CliActionResult(AsyncActionResultReturnCode));
        }

        public Task<int> ExecuteWithIntAsync()
        {
            return Task.FromResult(AsyncIntReturnCode);
        }

        public Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }

        public CliActionResult ExecuteWithActionResult()
        {
            return new CliActionResult(ActionResultReturnCode);
        }

        public int ExecuteWithInt()
        {
            return IntReturnCode;
        }

        public void Execute()
        {
            // Do Nothing
        }

        public string ExecuteInvalidReturnType()
        {
            return string.Empty;
        }
    }
}
