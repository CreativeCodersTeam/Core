using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Update command base that delegates to <see cref="ICrudRepository{TEntity, TKey}"/>.
/// </summary>
[PublicAPI]
public abstract class RepositoryUpdateCommandBase<TEntity, TKey, TOptions>
    : UpdateCommandBase<TEntity, TKey, TOptions>
    where TOptions : class, IKeyedOptions<TKey>
{
    private readonly ICrudRepository<TEntity, TKey> _repository;

    /// <summary>Initializes a new instance.</summary>
    protected RepositoryUpdateCommandBase(
        ICrudRepository<TEntity, TKey> repository,
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<TEntity?>> formatters)
        : base(confirmationPrompt, interactivePrompter, console, formatters)
    {
        _repository = Ensure.NotNull(repository);
    }

    /// <inheritdoc />
    protected override Task<TEntity?> LoadByKeyAsync(TKey key, CancellationToken cancellationToken)
        => _repository.GetAsync(key, cancellationToken);

    /// <inheritdoc />
    protected override Task<TEntity> PersistAsync(TKey key, TEntity entity,
        CancellationToken cancellationToken)
        => _repository.UpdateAsync(key, entity, cancellationToken);
}
