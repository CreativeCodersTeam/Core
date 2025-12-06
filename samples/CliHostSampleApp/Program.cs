using CreativeCoders.Cli.Hosting;

namespace CliHostSampleApp;

public static class Program
{
    static async Task Main(string[] args)
    {
        await CreateCliHost().RunAsync(args).ConfigureAwait(false);
    }

    private static ICliHost CreateCliHost()
    {
        return CliHostBuilder.Create()
            .Build();
    }
}
