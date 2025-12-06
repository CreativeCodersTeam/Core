using CreativeCoders.Cli.Hosting.Commands;

namespace CreativeCoders.Cli.Hosting;

public static class CliHostBuilder
{
    public static ICliHostBuilder Create()
    {
        return new DefaultCliHostBuilder(new AssemblyCommandScanner());
    }
}
