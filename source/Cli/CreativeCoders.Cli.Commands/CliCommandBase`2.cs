using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Options;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Cli.Core;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands;

/// <summary>
/// Result-producing variant of <see cref="CliCommandBase{TOptions}"/>. After
/// <see cref="OnExecuteAsync"/> returns, the result is rendered using an
/// <see cref="IOutputFormatter{TResult}"/> selected from DI when
/// <typeparamref name="TOptions"/> implements <see cref="IOutputFormatOptions"/>.
/// </summary>
/// <typeparam name="TOptions">The options type for this command.</typeparam>
/// <typeparam name="TResult">The result type produced by the command.</typeparam>
[PublicAPI]
public abstract class CliCommandBase<TOptions, TResult> : CliCommandBase<TOptions>
    where TOptions : class
{
    private readonly IEnumerable<IOutputFormatter<TResult>> _formatters;

    /// <summary>
    /// Initializes a new instance of <see cref="CliCommandBase{TOptions, TResult}"/>.
    /// </summary>
    /// <param name="confirmationPrompt">Service used to render confirmation prompts.</param>
    /// <param name="interactivePrompter">Service used for general-purpose prompts.</param>
    /// <param name="console">The console used for output.</param>
    /// <param name="formatters">All registered output formatters for <typeparamref name="TResult"/>.</param>
    protected CliCommandBase(
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<TResult>> formatters)
        : base(confirmationPrompt, interactivePrompter, console)
    {
        _formatters = Ensure.NotNull(formatters);
    }

    /// <inheritdoc />
    protected sealed override async Task<CommandResult> OnExecuteAsync(TOptions options,
        CancellationToken cancellationToken)
    {
        var result = await OnExecuteWithResultAsync(options, cancellationToken).ConfigureAwait(false);

        var commandResult = await OnAfterExecuteAsync(result, options, cancellationToken)
            .ConfigureAwait(false);

        if (commandResult is not null)
        {
            return commandResult;
        }

        if (options is IOutputFormatOptions formatOptions)
        {
            await WriteResultAsync(result, ResolveFormat(formatOptions.Format), cancellationToken)
                .ConfigureAwait(false);
        }

        return CommandResult.Success;
    }

    /// <summary>
    /// Executes the command and returns its result. The base class formats the result
    /// automatically when <typeparamref name="TOptions"/> implements <see cref="IOutputFormatOptions"/>.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    /// <returns>The result produced by the command.</returns>
    protected abstract Task<TResult> OnExecuteWithResultAsync(TOptions options,
        CancellationToken cancellationToken);

    /// <summary>
    /// Optional hook invoked after <see cref="OnExecuteAsync"/> and before output formatting.
    /// Return a <see cref="CommandResult"/> to short-circuit the default flow and skip output
    /// formatting. Return <see langword="null"/> to proceed with formatting and return
    /// <see cref="CommandResult.Success"/>.
    /// </summary>
    /// <param name="result">The result produced by <see cref="OnExecuteAsync"/>.</param>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    /// <returns>A non-success command result to short-circuit, or <see langword="null"/> to continue.</returns>
    protected virtual Task<CommandResult?> OnAfterExecuteAsync(TResult result, TOptions options,
        CancellationToken cancellationToken)
        => Task.FromResult<CommandResult?>(null);

    /// <summary>
    /// Renders <paramref name="result"/> using the formatter registered for <paramref name="format"/>.
    /// Falls back to <see cref="OutputFormat.Plain"/> when no formatter matches.
    /// </summary>
    /// <param name="result">The result to render.</param>
    /// <param name="format">The desired output format.</param>
    /// <param name="cancellationToken">A token cancelled by Ctrl+C.</param>
    protected virtual async Task WriteResultAsync(TResult result, OutputFormat format,
        CancellationToken cancellationToken)
    {
        var formatter = _formatters.FirstOrDefault(f => f.Format == format)
                        ?? _formatters.FirstOrDefault(f => f.Format == OutputFormat.Plain);

        if (formatter is null)
        {
            return;
        }

        await formatter.WriteAsync(result, Console, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Resolves the effective output format. When stdout is redirected and the requested format is
    /// the default <see cref="OutputFormat.Table"/>, falls back to <see cref="OutputFormat.Plain"/>
    /// to keep the output pipe-safe (no ANSI escapes, no table chrome).
    /// </summary>
    /// <param name="requested">The format requested via the options object.</param>
    /// <returns>The format to use.</returns>
    protected static OutputFormat ResolveFormat(OutputFormat requested)
    {
        if (requested == OutputFormat.Table && System.Console.IsOutputRedirected)
        {
            return OutputFormat.Plain;
        }

        return requested;
    }
}
