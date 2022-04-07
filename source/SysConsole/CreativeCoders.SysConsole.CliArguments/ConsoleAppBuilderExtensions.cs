using System;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.CliArguments.Building;

namespace CreativeCoders.SysConsole.CliArguments;

public static class ConsoleAppBuilderExtensions
{
    public static ConsoleAppBuilder UseCli(this ConsoleAppBuilder consoleAppBuilder,
        Action<ICliBuilder> setupCliBuilder)
    {
        return consoleAppBuilder.UseExecutor(sp =>
            new CliBuilderExecutor(setupCliBuilder, sp));
    }
}
