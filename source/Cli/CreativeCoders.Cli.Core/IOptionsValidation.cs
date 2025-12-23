namespace CreativeCoders.Cli.Core;

/// <summary>
/// Interface for command options validation.
/// </summary>
public interface IOptionsValidation
{
    /// <summary>
    /// Validates the command options asynchronously.
    /// </summary>
    /// <returns>A <see cref="OptionsValidationResult"/> indicating the result of the validation.</returns>
    Task<OptionsValidationResult> ValidateAsync();
}
