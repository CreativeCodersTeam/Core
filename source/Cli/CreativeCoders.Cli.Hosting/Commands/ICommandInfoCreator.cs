namespace CreativeCoders.Cli.Hosting.Commands;

/// <summary>
/// Defines a creator that builds <see cref="CliCommandInfo"/> instances from command types.
/// </summary>
public interface ICommandInfoCreator
{
    /// <summary>
    /// Creates a <see cref="CliCommandInfo"/> for the specified command type.
    /// </summary>
    /// <param name="commandType">The type of the CLI command to create info for.</param>
    /// <returns>A <see cref="CliCommandInfo"/> instance, or <see langword="null"/> if the type is not a valid CLI command.</returns>
    CliCommandInfo? Create(Type commandType);
}