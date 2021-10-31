using System;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.Core.Abstractions;

namespace CreativeCoders.SysConsole.App
{
    internal class DefaultConsoleApp : IConsoleApp
    {
        private readonly ISysConsole _sysConsole;

        private readonly ICommandExecutor _commandExecutor;

        private readonly string[] _args;

        private bool _reThrow;

        public DefaultConsoleApp(ICommandExecutor commandExecutor, string[] args, ISysConsole sysConsole)
        {
            _commandExecutor = Ensure.NotNull(commandExecutor, nameof(commandExecutor));
            _args = Ensure.NotNull(args, nameof(args));
            _sysConsole = Ensure.NotNull(sysConsole, nameof(sysConsole));
        }

        public IConsoleApp ReThrowExceptions(bool reThrow)
        {
            _reThrow = reThrow;

            return this;
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
                _sysConsole.WriteLineError(e.Message);

                if (_reThrow)
                {
                    throw;
                }

                return int.MinValue;
            }
        }
    }
}
