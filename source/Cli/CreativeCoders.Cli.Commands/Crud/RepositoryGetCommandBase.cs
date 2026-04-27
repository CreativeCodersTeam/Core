using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Get command base that delegates to <see cref="ICrudRepository{TEntity, TKey}.GetAsync"/>.
/// </summary>
[PublicAPI]
public abstract class RepositoryGetCommandBase<TEntity, TKey, TOptions>
    : GetCommandBase<TEntity, TKey, TOptions>
    where TOptions : class, IKeyedOptions<TKey>
{
    private readonly ICrudRepository<TEntity, TKey> _repository;

    /// <summary>Initializes a new instance.</summary>
    protected RepositoryGetCommandBase(
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
}
