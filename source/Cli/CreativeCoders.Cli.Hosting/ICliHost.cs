namespace CreativeCoders.Cli.Hosting;

public interface ICliHost
{
    Task<CliResult> RunAsync(string[] args);

    /// <summary>
    /// Runs the CLI host and returns the exit code as an integer.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The exit code of the executed command.</returns>
    Task<int> RunMainAsync(string[] args);
}
