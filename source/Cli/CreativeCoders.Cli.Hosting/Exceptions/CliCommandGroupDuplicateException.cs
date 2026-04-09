namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when duplicate command group attributes are detected during validation.
/// </summary>
public class CliCommandGroupDuplicateException()
    : CliCommandStructureValidationException("Group attribute duplicates found");
