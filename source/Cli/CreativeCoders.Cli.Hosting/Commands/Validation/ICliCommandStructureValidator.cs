using CreativeCoders.Cli.Hosting.Commands.Store;

namespace CreativeCoders.Cli.Hosting.Commands.Validation;

/// <summary>
/// Defines a validator for the CLI command tree structure.
/// </summary>
public interface ICliCommandStructureValidator
{
    /// <summary>
    /// Validates the command store structure for duplicates and ambiguous commands.
    /// </summary>
    /// <param name="commandStore">The command store to validate.</param>
    /// <exception cref="CreativeCoders.Cli.Hosting.Exceptions.CliCommandGroupDuplicateException">Duplicate group attributes were found.</exception>
    /// <exception cref="CreativeCoders.Cli.Hosting.Exceptions.AmbiguousCliCommandsException">Ambiguous command definitions were found.</exception>
    void Validate(ICliCommandStore commandStore);
}
