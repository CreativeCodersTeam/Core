using System;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.App;
using CreativeCoders.SysConsole.CliArguments.Building;

namespace CreativeCoders.SysConsole.CliArguments;

public class CliBuilderExecutor : IConsoleAppExecutor
{
    private readonly Action<ICliBuilder> _setupCliBuilder;

    private readonly IServiceProvider _serviceProvider;

    public CliBuilderExecutor(Action<ICliBuilder> setupCliBuilder, IServiceProvider serviceProvider)
    {
        _setupCliBuilder = Ensure.NotNull(setupCliBuilder, nameof(setupCliBuilder));
        _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        var cliBuilder = new DefaultCliBuilder(_serviceProvider);

        _setupCliBuilder(cliBuilder);

        var cliExecutor = cliBuilder.BuildExecutor();

        return await cliExecutor.ExecuteAsync(args).ConfigureAwait(false);
    }
}
