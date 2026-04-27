using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Confirmation;

/// <summary>
/// Default <see cref="IConfirmationPrompt"/> implementation backed by
/// <see cref="Spectre.Console.IAnsiConsole"/>.
/// </summary>
[PublicAPI]
public sealed class SpectreConfirmationPrompt : IConfirmationPrompt
{
    private readonly IAnsiConsole _console;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpectreConfirmationPrompt"/> class.
    /// </summary>
    /// <param name="console">The console used to render the prompt.</param>
    public SpectreConfirmationPrompt(IAnsiConsole console)
    {
        _console = Ensure.NotNull(console);
    }

    /// <inheritdoc />
    public async Task<bool> ConfirmAsync(string message, bool defaultValue,
        CancellationToken cancellationToken)
    {
        Ensure.IsNotNullOrWhitespace(message);

        var prompt = new ConfirmationPrompt(message)
        {
            DefaultValue = defaultValue
        };

        return await _console
            .PromptAsync(prompt, cancellationToken)
            .ConfigureAwait(false);
    }
}
