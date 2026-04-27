using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Options;
using CreativeCoders.Cli.Core;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands;

/// <summary>
/// Base class for <see cref="ICliCommand{TOptions}"/> implementations that provides cross-cutting
/// behavior driven by marker interfaces on the options type:
/// <see cref="IVerbosityOptions"/>, <see cref="IInteractiveOptions"/>,
/// <see cref="IConfirmableOptions"/>, and <see cref="IDryRunOptions"/>.
/// </summary>
/// <typeparam name="TOptions">The options type for this command.</typeparam>
[PublicAPI]
public abstract class CliCommandBase<TOptions> : ICliCommand<TOptions>
    where TOptions : class
{
    private readonly IConfirmationPrompt _confirmationPrompt;
    private readonly IInteractivePrompter _interactivePrompter;

    /// <summary>The console used by helper prompt methods and overridable hooks.</summary>
    protected IAnsiConsole Console { get; }

    /// <summary>The interactive prompter exposed for helper methods.</summary>
    protected IInteractivePrompter Prompter => _interactivePrompter;

    /// <summary>
    /// Initializes a new instance of <see cref="CliCommandBase{TOptions}"/>.
    /// </summary>
    /// <param name="confirmationPrompt">Service used to render confirmation prompts.</param>
    /// <param name="interactivePrompter">Service used for general-purpose prompts.</param>
    /// <param name="console">The console used for output.</param>
    protected CliCommandBase(
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console)
    {
        _confirmationPrompt = Ensure.NotNull(confirmationPrompt);
        _interactivePrompter = Ensure.NotNull(interactivePrompter);
        Console = Ensure.NotNull(console);
    }

    /// <inheritdoc />
    public async Task<CommandResult> ExecuteAsync(TOptions options)
    {
        Ensure.NotNull(options);

        using var cts = new CancellationTokenSource();

        ConsoleCancelEventHandler handler = (_, args) =>
        {
            args.Cancel = true;
            // ReSharper disable once AccessToDisposedClosure
            cts.Cancel();
        };

        System.Console.CancelKeyPress += handler;

        try
        {
            return await ExecuteCoreAsync(options, cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return new CommandResult(ExitCodes.Cancelled);
        }
        catch (Exception ex)
        {
            return await OnHandleExceptionAsync(ex, options).ConfigureAwait(false);
        }
        finally
        {
            System.Console.CancelKeyPress -= handler;
        }
    }

    private async Task<CommandResult> ExecuteCoreAsync(TOptions options, CancellationToken cancellationToken)
    {
        if (options is IInteractiveOptions interactiveOptions
            && _interactivePrompter.IsInteractive
            && !interactiveOptions.NoInteractive)
        {
            await PromptMissingOptionsAsync(options, _interactivePrompter, cancellationToken)
                .ConfigureAwait(false);
        }

        if (options is IConfirmableOptions confirmable
            && !confirmable.Yes
            && RequiresConfirmation(options))
        {
            var confirmed = await _confirmationPrompt
                .ConfirmAsync(GetConfirmationMessage(options), defaultValue: false, cancellationToken)
                .ConfigureAwait(false);

            if (!confirmed)
            {
                return new CommandResult(ExitCodes.Cancelled);
            }
        }

        if (options is IDryRunOptions { DryRun: true })
        {
            await OnDryRunAsync(options, cancellationToken).ConfigureAwait(false);

            return CommandResult.Success;
        }

        return await OnExecuteAsync(options, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the command. Implementations contain the actual business logic.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    /// <returns>The command result.</returns>
    protected abstract Task<CommandResult> OnExecuteAsync(TOptions options,
        CancellationToken cancellationToken);

    /// <summary>
    /// Invoked instead of <see cref="OnExecuteAsync"/> when <see cref="IDryRunOptions.DryRun"/> is
    /// <see langword="true"/>. Default writes a generic message; override to describe the no-op.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    protected virtual Task OnDryRunAsync(TOptions options, CancellationToken cancellationToken)
    {
        Console.MarkupLine("[yellow]Dry run: no changes performed.[/]");

        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns the message displayed by the confirmation prompt when
    /// <typeparamref name="TOptions"/> implements <see cref="IConfirmableOptions"/>.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <returns>The confirmation message.</returns>
    protected virtual string GetConfirmationMessage(TOptions options)
        => "Are you sure you want to continue?";

    /// <summary>
    /// Indicates whether the command should ask for confirmation. Defaults to <see langword="true"/>
    /// whenever the options implement <see cref="IConfirmableOptions"/>; override to opt out
    /// conditionally.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <returns><see langword="true"/> to prompt for confirmation; <see langword="false"/> to skip.</returns>
    protected virtual bool RequiresConfirmation(TOptions options) => true;

    /// <summary>
    /// Handles an unhandled exception thrown by the template flow. Default implementation prints
    /// the message in red and returns <see cref="ExitCodes.Error"/>.
    /// </summary>
    /// <param name="exception">The thrown exception.</param>
    /// <param name="options">The command options.</param>
    /// <returns>The command result to return.</returns>
    protected virtual Task<CommandResult> OnHandleExceptionAsync(Exception exception, TOptions options)
    {
        Ensure.NotNull(exception);

        Console.MarkupLineInterpolated($"[red]Error:[/] {exception.Message}");

        return Task.FromResult(new CommandResult(ExitCodes.Error));
    }

    /// <summary>
    /// Override to fill in missing required properties on <paramref name="options"/> by prompting
    /// the user. Invoked only when <typeparamref name="TOptions"/> implements
    /// <see cref="IInteractiveOptions"/>, the process runs on a TTY, and
    /// <see cref="IInteractiveOptions.NoInteractive"/> is <see langword="false"/>.
    /// </summary>
    /// <param name="options">The command options to fill in.</param>
    /// <param name="prompter">The prompter to use.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    protected virtual Task PromptMissingOptionsAsync(TOptions options, IInteractivePrompter prompter,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <summary>Convenience wrapper around <see cref="IInteractivePrompter.PromptAsync{T}"/>.</summary>
    /// <typeparam name="T">The expected value type.</typeparam>
    /// <param name="message">The prompt message.</param>
    /// <param name="defaultValue">An optional default value.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    /// <returns>The user's response.</returns>
    protected Task<T> PromptAsync<T>(string message, T? defaultValue = default,
        CancellationToken cancellationToken = default)
        => _interactivePrompter.PromptAsync(message, defaultValue, cancellationToken);

    /// <summary>Convenience wrapper around <see cref="IInteractivePrompter.ConfirmAsync"/>.</summary>
    /// <param name="message">The prompt message.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    /// <returns>The user's response.</returns>
    protected Task<bool> ConfirmAsync(string message, bool defaultValue = false,
        CancellationToken cancellationToken = default)
        => _interactivePrompter.ConfirmAsync(message, defaultValue, cancellationToken);

    /// <summary>Convenience wrapper around <see cref="IInteractivePrompter.SelectAsync{T}"/>.</summary>
    /// <typeparam name="T">The choice type.</typeparam>
    /// <param name="message">The prompt message.</param>
    /// <param name="choices">The available choices.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    /// <returns>The selected item.</returns>
    protected Task<T> SelectAsync<T>(string message, IEnumerable<T> choices,
        CancellationToken cancellationToken = default)
        where T : notnull
        => _interactivePrompter.SelectAsync(message, choices, cancellationToken);

    /// <summary>Convenience wrapper around <see cref="IInteractivePrompter.MultiSelectAsync{T}"/>.</summary>
    /// <typeparam name="T">The choice type.</typeparam>
    /// <param name="message">The prompt message.</param>
    /// <param name="choices">The available choices.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    /// <returns>The selected items.</returns>
    protected Task<IReadOnlyList<T>> MultiSelectAsync<T>(string message, IEnumerable<T> choices,
        CancellationToken cancellationToken = default)
        where T : notnull
        => _interactivePrompter.MultiSelectAsync(message, choices, cancellationToken);
}
