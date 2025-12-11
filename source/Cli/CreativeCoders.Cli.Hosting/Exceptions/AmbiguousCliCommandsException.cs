namespace CreativeCoders.Cli.Hosting.Exceptions;

public class AmbiguousCliCommandsException()
    : CliCommandStructureValidationException("Ambiguous commands found") { }
