using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Cli.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Base class for "remove" commands. Defaults to requiring confirmation; the default confirmation
/// message references the key. Override <see cref="CliCommandBase{TOptions}.GetConfirmationMessage"/>
/// for custom wording.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TOptions">The options type. Must implement <see cref="IKeyedOptions{TKey}"/>.</typeparam>
[PublicAPI]
public abstract class RemoveCommandBase<TEntity, TKey, TOptions>
    : CliCommandBase<TOptions>
    where TOptions : class, IKeyedOptions<TKey>
{
    /// <summary>Initializes a new instance of <see cref="RemoveCommandBase{TEntity, TKey, TOptions}"/>.</summary>
    protected RemoveCommandBase(
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console)
        : base(confirmationPrompt, interactivePrompter, console) { }

    /// <inheritdoc />
    protected override async Task<CommandResult> OnExecuteAsync(TOptions options,
        CancellationToken cancellationToken)
    {
        await RemoveByKeyAsync(options.Key, cancellationToken).ConfigureAwait(false);

        return CommandResult.Success;
    }

    /// <inheritdoc />
    protected override string GetConfirmationMessage(TOptions options)
        => $"Really remove '{options.Key}'?";

    /// <inheritdoc />
    protected override bool RequiresConfirmation(TOptions options) => true;

    /// <summary>Removes the entity with the given key.</summary>
    /// <param name="key">The key of the entity to remove.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    protected abstract Task RemoveByKeyAsync(TKey key, CancellationToken cancellationToken);
}
