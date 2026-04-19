namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when the CLI command structure validation fails.
/// </summary>
/// <param name="message">The validation error message.</param>
public class CliCommandStructureValidationException(string message) : Exception(message);
