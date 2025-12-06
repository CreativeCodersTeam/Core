using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;

namespace CreativeCoders.Cli.Hosting;

public class DefaultCliHost : ICliHost
{
    public DefaultCliHost(CliCommandStore commandStore)
    {
        throw new NotImplementedException();
    }

    public Task<CliResult> RunAsync(string[] args)
    {
        throw new NotImplementedException();
    }
}
