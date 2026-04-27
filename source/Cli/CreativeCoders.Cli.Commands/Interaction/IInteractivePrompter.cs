using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Interaction;

/// <summary>
/// Provides interactive prompts (text, confirm, select, multi-select) for CLI commands.
/// Implementations may throw <see cref="InvalidOperationException"/> when no TTY is available.
/// </summary>
[PublicAPI]
public interface IInteractivePrompter
{
    /// <summary>
    /// Gets a value indicating whether the current process is running on an interactive TTY.
    /// </summary>
    bool IsInteractive { get; }

    /// <summary>
    /// Prompts the user for a single typed value.
    /// </summary>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <param name="message">The message to display.</param>
    /// <param name="defaultValue">An optional default value.</param>
    /// <param name="cancellationToken">Token to cancel the prompt.</param>
    /// <returns>The value entered by the user.</returns>
    Task<T> PromptAsync<T>(string message, T? defaultValue, CancellationToken cancellationToken);

    /// <summary>
    /// Prompts the user with a yes/no question.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="defaultValue">The default value used on accepting without input.</param>
    /// <param name="cancellationToken">Token to cancel the prompt.</param>
    /// <returns>The user's response.</returns>
    Task<bool> ConfirmAsync(string message, bool defaultValue, CancellationToken cancellationToken);

    /// <summary>
    /// Prompts the user to select a single item from the given choices.
    /// </summary>
    /// <typeparam name="T">The choice type.</typeparam>
    /// <param name="message">The message to display.</param>
    /// <param name="choices">The available choices. Must contain at least one element.</param>
    /// <param name="cancellationToken">Token to cancel the prompt.</param>
    /// <returns>The selected item.</returns>
    Task<T> SelectAsync<T>(string message, IEnumerable<T> choices, CancellationToken cancellationToken)
        where T : notnull;

    /// <summary>
    /// Prompts the user to select zero or more items from the given choices.
    /// </summary>
    /// <typeparam name="T">The choice type.</typeparam>
    /// <param name="message">The message to display.</param>
    /// <param name="choices">The available choices.</param>
    /// <param name="cancellationToken">Token to cancel the prompt.</param>
    /// <returns>The selected items.</returns>
    Task<IReadOnlyList<T>> MultiSelectAsync<T>(string message, IEnumerable<T> choices,
        CancellationToken cancellationToken)
        where T : notnull;
}
