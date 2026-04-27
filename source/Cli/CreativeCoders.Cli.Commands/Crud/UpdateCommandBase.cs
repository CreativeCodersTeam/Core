using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Cli.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Base class for "update" commands. Loads an entity by key, lets the implementation construct the
/// new value, and persists it.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TOptions">The options type. Must implement <see cref="IKeyedOptions{TKey}"/>.</typeparam>
[PublicAPI]
public abstract class UpdateCommandBase<TEntity, TKey, TOptions>
    : CliCommandBase<TOptions, TEntity?>
    where TOptions : class, IKeyedOptions<TKey>
{
    /// <summary>Initializes a new instance of <see cref="UpdateCommandBase{TEntity, TKey, TOptions}"/>.</summary>
    protected UpdateCommandBase(
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<TEntity?>> formatters)
        : base(confirmationPrompt, interactivePrompter, console, formatters) { }

    /// <inheritdoc />
    protected override async Task<TEntity?> OnExecuteWithResultAsync(TOptions options,
        CancellationToken cancellationToken)
    {
        var existing = await LoadByKeyAsync(options.Key, cancellationToken).ConfigureAwait(false);

        if (existing is null)
        {
            return default;
        }

        var updated = await BuildEntityAsync(options, existing, cancellationToken)
            .ConfigureAwait(false);

        return await PersistAsync(options.Key, updated, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override Task<CommandResult?> OnAfterExecuteAsync(TEntity? result, TOptions options,
        CancellationToken cancellationToken)
    {
        if (result is null)
        {
            Console.MarkupLineInterpolated($"[yellow]Not found:[/] {options.Key}");

            return Task.FromResult<CommandResult?>(new CommandResult(ExitCodes.NotFound));
        }

        return Task.FromResult<CommandResult?>(null);
    }

    /// <summary>Loads the existing entity by key.</summary>
    protected abstract Task<TEntity?> LoadByKeyAsync(TKey key, CancellationToken cancellationToken);

    /// <summary>
    /// Builds the new entity value. Default implementation prefers
    /// <see cref="IEntityInputOptions{TEntity}.Entity"/> when present; otherwise must be overridden.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="existing">The existing entity (already loaded).</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The new entity value.</returns>
    protected virtual Task<TEntity> BuildEntityAsync(TOptions options, TEntity existing,
        CancellationToken cancellationToken)
    {
        if (options is IEntityInputOptions<TEntity> input)
        {
            return Task.FromResult(input.Entity);
        }

        throw new InvalidOperationException(
            $"{GetType().Name} must override {nameof(BuildEntityAsync)} or implement " +
            $"IEntityInputOptions<{typeof(TEntity).Name}> on the options type.");
    }

    /// <summary>Persists the updated entity and returns the persisted form.</summary>
    protected abstract Task<TEntity> PersistAsync(TKey key, TEntity entity,
        CancellationToken cancellationToken);
}
