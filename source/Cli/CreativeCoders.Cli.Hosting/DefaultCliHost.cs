using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.Cli.Hosting;

public class DefaultCliHost : ICliHost
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ICliCommandStore _commandStore;

    public DefaultCliHost(ICliCommandStore commandStore, IServiceProvider serviceProvider)
    {
        _serviceProvider = Ensure.NotNull(serviceProvider);
        _commandStore = Ensure.NotNull(commandStore);
    }

    public async Task<CliResult> RunAsync(string[] args)
    {
        var commandInfo = _commandStore.FindCommandForArgs(args);
        if (commandInfo != null)
        {
            var command = commandInfo.CommandType.CreateInstance<ICliCommand>(_serviceProvider);

            if (command != null)
            {
                var result = await command.ExecuteAsync().ConfigureAwait(false);

                return new CliResult
                {
                    ExitCode = result.ExitCode
                };
            }
        }

        return new CliResult
        {
            ExitCode = int.MinValue
        };
    }
}
