using System;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.SysConsole.App.MainProgram
{
    public static class ConsoleAppBuilderExtensions
    {
        public static ConsoleAppBuilder UseProgramMain<TProgramMain>(this ConsoleAppBuilder consoleAppBuilder)
            where TProgramMain : IMain
        {
            return consoleAppBuilder.UseExecutor(sp =>
            {
                var main = typeof(TProgramMain).CreateInstance<IMain>(sp);

                if (main == null)
                {
                    throw new InvalidOperationException();
                }

                return new MainExecutor(main);
            });
        }
    }
}
