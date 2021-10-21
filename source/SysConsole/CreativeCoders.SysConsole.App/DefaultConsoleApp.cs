using System;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.App
{
    internal class DefaultConsoleApp : IConsoleApp
    {
        private readonly IMain _main;

        public DefaultConsoleApp(IMain main)
        {
            _main = Ensure.NotNull(main, nameof(main));
        }

        public async Task<int> RunAsync()
        {
            try
            {
                var result = await _main.ExecuteAsync().ConfigureAwait(false);

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
