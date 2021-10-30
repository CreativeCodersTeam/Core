using System;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.App
{
    internal class DefaultConsoleApp : IConsoleApp
    {
        private readonly ICommandExecutor _commandExecutor;

        private readonly string[] _args;

        public DefaultConsoleApp(ICommandExecutor commandExecutor, string[] args)
        {
            _commandExecutor = Ensure.NotNull(commandExecutor, nameof(commandExecutor));
            _args = Ensure.NotNull(args, nameof(args));
        }

        public async Task<int> RunAsync()
        {
            try
            {
                var result = await _commandExecutor.ExecuteAsync(_args).ConfigureAwait(false);

                return result;
            }
            catch (ConsoleException e)
            {
                return e.ReturnCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return int.MinValue;
            }
        }
    }
}
