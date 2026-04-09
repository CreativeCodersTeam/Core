using CreativeCoders.Cli.Core;
using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when command options fail validation.
/// </summary>
/// <param name="validationResult">The validation result containing the failure details.</param>
public class CliCommandOptionsInvalidException(OptionsValidationResult validationResult)
    : CliExitException("Command options are invalid", CliExitCodes.CommandOptionsInvalid)
{
    /// <summary>
    /// Gets the validation result containing the failure messages.
    /// </summary>
    /// <value>The <see cref="OptionsValidationResult"/> describing the validation failures.</value>
    public OptionsValidationResult ValidationResult { get; } = Ensure.NotNull(validationResult);
}
