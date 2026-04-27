using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Interaction;

/// <summary>
/// Default <see cref="IInteractivePrompter"/> implementation backed by Spectre.Console.
/// Throws <see cref="InvalidOperationException"/> when no interactive TTY is available
/// to prevent CI jobs from hanging on a prompt.
/// </summary>
[PublicAPI]
public sealed class SpectreInteractivePrompter : IInteractivePrompter
{
    private readonly IAnsiConsole _console;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpectreInteractivePrompter"/> class.
    /// </summary>
    /// <param name="console">The console used to render prompts.</param>
    public SpectreInteractivePrompter(IAnsiConsole console)
    {
        _console = Ensure.NotNull(console);
    }

    /// <inheritdoc />
    public bool IsInteractive => !Console.IsInputRedirected && !Console.IsOutputRedirected;

    /// <inheritdoc />
    public Task<T> PromptAsync<T>(string message, T? defaultValue, CancellationToken cancellationToken)
    {
        Ensure.IsNotNullOrWhitespace(message);
        EnsureInteractive();

        var prompt = new TextPrompt<T>(message);

        if (defaultValue is not null)
        {
            prompt.DefaultValue(defaultValue);
        }

        return _console.PromptAsync(prompt, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> ConfirmAsync(string message, bool defaultValue, CancellationToken cancellationToken)
    {
        Ensure.IsNotNullOrWhitespace(message);
        EnsureInteractive();

        var prompt = new ConfirmationPrompt(message)
        {
            DefaultValue = defaultValue
        };

        return _console.PromptAsync(prompt, cancellationToken);
    }

    /// <inheritdoc />
    public Task<T> SelectAsync<T>(string message, IEnumerable<T> choices,
        CancellationToken cancellationToken)
        where T : notnull
    {
        Ensure.IsNotNullOrWhitespace(message);
        Ensure.NotNull(choices);
        EnsureInteractive();

        var prompt = new SelectionPrompt<T>()
            .Title(message)
            .AddChoices(choices);

        return _console.PromptAsync(prompt, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<T>> MultiSelectAsync<T>(string message, IEnumerable<T> choices,
        CancellationToken cancellationToken)
        where T : notnull
    {
        Ensure.IsNotNullOrWhitespace(message);
        Ensure.NotNull(choices);
        EnsureInteractive();

        var prompt = new MultiSelectionPrompt<T>()
            .Title(message)
            .AddChoices(choices);

        var result = await _console
            .PromptAsync(prompt, cancellationToken)
            .ConfigureAwait(false);

        return result;
    }

    private void EnsureInteractive()
    {
        if (!IsInteractive)
        {
            throw new InvalidOperationException(
                "Interactive prompt requires a TTY. Run the command in a terminal or supply the value " +
                "via a CLI argument.");
        }
    }
}
