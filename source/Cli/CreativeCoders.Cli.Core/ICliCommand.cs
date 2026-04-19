namespace CreativeCoders.Cli.Core;

/// <summary>
/// Defines a CLI command that accepts options of type <typeparamref name="TOptions"/>.
/// </summary>
/// <typeparam name="TOptions">The type of the options passed to the command.</typeparam>
public interface ICliCommand<in TOptions>
    where TOptions : class
{
    /// <summary>
    /// Executes the CLI command asynchronously with the specified options.
    /// </summary>
    /// <param name="options">The options for the command.</param>
    /// <returns>A <see cref="CommandResult"/> representing the outcome of the command execution.</returns>
    Task<CommandResult> ExecuteAsync(TOptions options);
}

/// <summary>
/// Defines a CLI command without options.
/// </summary>
public interface ICliCommand
{
    /// <summary>
    /// Executes the CLI command asynchronously.
    /// </summary>
    /// <returns>A <see cref="CommandResult"/> representing the outcome of the command execution.</returns>
    Task<CommandResult> ExecuteAsync();
}
