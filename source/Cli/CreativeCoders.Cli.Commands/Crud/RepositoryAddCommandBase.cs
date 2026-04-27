using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Add command base that delegates to <see cref="ICrudRepository{TEntity, TKey}.AddAsync"/>.
/// </summary>
[PublicAPI]
public abstract class RepositoryAddCommandBase<TEntity, TKey, TOptions>
    : AddCommandBase<TEntity, TOptions>
    where TOptions : class
{
    private readonly ICrudRepository<TEntity, TKey> _repository;

    /// <summary>Initializes a new instance.</summary>
    protected RepositoryAddCommandBase(
        ICrudRepository<TEntity, TKey> repository,
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<TEntity>> formatters)
        : base(confirmationPrompt, interactivePrompter, console, formatters)
    {
        _repository = Ensure.NotNull(repository);
    }

    /// <inheritdoc />
    protected override Task<TEntity> PersistAsync(TEntity entity, CancellationToken cancellationToken)
        => _repository.AddAsync(entity, cancellationToken);
}
