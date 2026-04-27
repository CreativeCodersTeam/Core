using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Base class for "add" commands. Builds an entity (either from
/// <see cref="IEntityInputOptions{TEntity}"/> or by overriding <see cref="BuildEntityAsync"/>)
/// and persists it via <see cref="PersistAsync"/>.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TOptions">The options type.</typeparam>
[PublicAPI]
public abstract class AddCommandBase<TEntity, TOptions>
    : CliCommandBase<TOptions, TEntity>
    where TOptions : class
{
    /// <summary>Initializes a new instance of <see cref="AddCommandBase{TEntity, TOptions}"/>.</summary>
    protected AddCommandBase(
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<TEntity>> formatters)
        : base(confirmationPrompt, interactivePrompter, console, formatters) { }

    /// <inheritdoc />
    protected override async Task<TEntity> OnExecuteWithResultAsync(TOptions options,
        CancellationToken cancellationToken)
    {
        var entity = await BuildEntityAsync(options, cancellationToken).ConfigureAwait(false);

        return await PersistAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Builds the entity to persist. Default implementation uses
    /// <see cref="IEntityInputOptions{TEntity}.Entity"/> when available; otherwise must be
    /// overridden.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The entity to persist.</returns>
    protected virtual Task<TEntity> BuildEntityAsync(TOptions options, CancellationToken cancellationToken)
    {
        if (options is IEntityInputOptions<TEntity> input)
        {
            return Task.FromResult(input.Entity);
        }

        throw new InvalidOperationException(
            $"{GetType().Name} must override {nameof(BuildEntityAsync)} or implement " +
            $"IEntityInputOptions<{typeof(TEntity).Name}> on the options type.");
    }

    /// <summary>Persists the entity and returns the persisted form.</summary>
    /// <param name="entity">The entity to persist.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The persisted entity.</returns>
    protected abstract Task<TEntity> PersistAsync(TEntity entity, CancellationToken cancellationToken);

    /// <inheritdoc />
    protected override bool RequiresConfirmation(TOptions options) => false;
}
