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

    /// <summary>
    /// Creates a predefined valid result for options validation where
    /// the validation is successful without any messages.
    /// </summary>
    /// <returns>
    /// An <see cref="OptionsValidationResult"/> instance representing a successful validation result
    /// with an empty collection of messages.
    /// </returns>
    public static OptionsValidationResult Valid() => new OptionsValidationResult(true);

    /// <summary>
    /// Creates a predefined invalid result for options validation where
    /// the validation fails and may include associated validation messages.
    /// </summary>
    /// <param name="messages">An optional collection of validation messages explaining the failure.
    /// If null, the result will contain an empty collection of messages.</param>
    /// <returns>
    /// An <see cref="OptionsValidationResult"/> instance representing an unsuccessful validation result
    /// with the provided validation messages or an empty collection if none are specified.
    /// </returns>
    public static OptionsValidationResult Invalid(IEnumerable<string>? messages)
        => new OptionsValidationResult(false, messages);
}
