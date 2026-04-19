namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when ambiguous CLI command definitions are detected during validation.
/// </summary>
public class AmbiguousCliCommandsException()
    : CliCommandStructureValidationException("Ambiguous commands found");
