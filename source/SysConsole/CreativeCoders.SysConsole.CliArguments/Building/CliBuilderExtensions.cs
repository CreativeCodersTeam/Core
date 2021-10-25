using System;
using System.Threading.Tasks;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Building
{
    public static class CliBuilderExtensions
    {
        public static ICliBuilder AddCommand<TOptions>(this ICliBuilder cliBuilder,
            string name, Func<TOptions, Task<CliCommandResult>> executeAsync)
            where TOptions : class, new()
        {
            cliBuilder.AddCommand<DelegateCliCommand<TOptions>, TOptions>(x =>
            {
                x.Name = name;
                x.OnExecuteAsync = executeAsync;
            });

            return cliBuilder;
        }
    }
}
