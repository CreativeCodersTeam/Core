using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Cli.Hosting;

[ExcludeFromCodeCoverage]
public static class CliHostBuilder
{
    public static ICliHostBuilder Create()
    {
        return new DefaultCliHostBuilder();
    }
}
