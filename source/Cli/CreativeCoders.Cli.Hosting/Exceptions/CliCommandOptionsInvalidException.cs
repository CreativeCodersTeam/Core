using CreativeCoders.Cli.Core;
using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliCommandOptionsInvalidException(OptionsValidationResult validationResult)
    : CliExitException("Command options are invalid", CliExitCodes.CommandOptionsInvalid)
{
    public OptionsValidationResult ValidationResult { get; } = Ensure.NotNull(validationResult);
}
