using CreativeCoders.Cli.Hosting;
using CreativeCoders.Cli.Hosting.Help;

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
            .EnableHelp(HelpCommandKind.CommandOrArgument)
            .PrintHeaderMarkup(["[red]This is a sample cli host[/]"])
            .Build();
    }
}
