using System;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.SysConsole.App.MainProgram;

public static class ConsoleAppBuilderExtensions
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Registers an executor for a main program. </summary>
    ///
    /// <exception cref="InvalidOperationException">    Thrown when the program main class could not
    ///                                                 be created. </exception>
    ///
    /// <typeparam name="TProgramMain"> Type of the program main class. </typeparam>
    /// <param name="consoleAppBuilder">    The consoleAppBuilder to act on. </param>
    ///
    /// <returns>   The ConsoleAppBuilder. </returns>
    ///-------------------------------------------------------------------------------------------------
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