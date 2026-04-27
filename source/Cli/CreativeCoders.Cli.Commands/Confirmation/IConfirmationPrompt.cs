using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Confirmation;

/// <summary>
/// Asks the user to confirm or decline an action.
/// </summary>
[PublicAPI]
public interface IConfirmationPrompt
{
    /// <summary>
    /// Prompts the user with the given message and returns the user's response.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="defaultValue">The value used when the user accepts the prompt without input.</param>
    /// <param name="cancellationToken">Token to cancel the prompt.</param>
    /// <returns>
    /// <see langword="true"/> if the user confirms; <see langword="false"/> if the user declines.
    /// </returns>
    Task<bool> ConfirmAsync(string message, bool defaultValue, CancellationToken cancellationToken);
}
