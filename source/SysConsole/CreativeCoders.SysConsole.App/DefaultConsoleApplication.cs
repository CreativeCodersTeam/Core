using System;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.App
{
    internal class DefaultConsoleApplication : IConsoleApplication
    {
        private readonly IMain _main;

        public DefaultConsoleApplication(IMain main)
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
