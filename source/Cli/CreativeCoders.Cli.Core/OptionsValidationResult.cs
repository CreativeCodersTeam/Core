namespace CreativeCoders.Cli.Core;

/// <summary>
/// Represents the result of a command options validation.
/// </summary>
/// <param name="isValid">A boolean value indicating whether the options are valid.</param>
/// <param name="messages">An optional collection of validation messages.</param>
public class OptionsValidationResult(bool isValid, IEnumerable<string>? messages = null)
{
    /// <summary>
    /// Gets a value indicating whether the validation was successful.
    /// </summary>
    public bool IsValid { get; } = isValid;

    /// <summary>
    /// Gets the validation messages.
    /// </summary>
    public IEnumerable<string> Messages { get; } = messages ?? [];

    public static OptionsValidationResult Valid() => new OptionsValidationResult(true);

    public static OptionsValidationResult Invalid(IEnumerable<string>? messages)
        => new OptionsValidationResult(false, messages);
}
