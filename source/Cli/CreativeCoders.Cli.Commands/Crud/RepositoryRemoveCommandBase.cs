using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Remove command base that delegates to <see cref="ICrudRepository{TEntity, TKey}.RemoveAsync"/>.
/// </summary>
[PublicAPI]
public abstract class RepositoryRemoveCommandBase<TEntity, TKey, TOptions>
    : RemoveCommandBase<TEntity, TKey, TOptions>
    where TOptions : class, IKeyedOptions<TKey>
{
    private readonly ICrudRepository<TEntity, TKey> _repository;

    /// <summary>Initializes a new instance.</summary>
    protected RepositoryRemoveCommandBase(
        ICrudRepository<TEntity, TKey> repository,
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console)
        : base(confirmationPrompt, interactivePrompter, console)
    {
        _repository = Ensure.NotNull(repository);
    }

    /// <inheritdoc />
    protected override Task RemoveByKeyAsync(TKey key, CancellationToken cancellationToken)
        => _repository.RemoveAsync(key, cancellationToken);
}
