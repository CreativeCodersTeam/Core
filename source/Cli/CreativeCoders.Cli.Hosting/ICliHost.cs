using JetBrains.Annotations;

namespace CreativeCoders.Cli.Hosting;

/// <summary>
/// Represents a Command Line Interface (CLI) host that provides functionality to execute
/// commands and handle related tasks.
/// </summary>
[PublicAPI]
public interface ICliHost
{
    /// <summary>
    /// Executes the CLI host with the given arguments and returns the result of the operation.
    /// </summary>
    /// <param name="args">The command line arguments provided to the host.</param>
    /// <returns>A <see cref="CliResult"/> object containing the exit code of the executed command.</returns>
    Task<CliResult> RunAsync(string[] args);

    /// <summary>
    /// Runs the CLI host and returns the exit code as an integer.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The exit code of the executed command.</returns>
    Task<int> RunMainAsync(string[] args);
}
