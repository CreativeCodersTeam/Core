using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// List command base that delegates to <see cref="ICrudRepository{TEntity, TKey}.ListAsync"/>.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TOptions">The options type.</typeparam>
[PublicAPI]
public abstract class RepositoryListCommandBase<TEntity, TKey, TOptions>
    : ListCommandBase<TEntity, TOptions>
    where TOptions : class
{
    private readonly ICrudRepository<TEntity, TKey> _repository;

    /// <summary>Initializes a new instance.</summary>
    protected RepositoryListCommandBase(
        ICrudRepository<TEntity, TKey> repository,
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<IReadOnlyList<TEntity>>> formatters)
        : base(confirmationPrompt, interactivePrompter, console, formatters)
    {
        _repository = Ensure.NotNull(repository);
    }

    /// <inheritdoc />
    protected override Task<IReadOnlyList<TEntity>> LoadAsync(TOptions options,
        CancellationToken cancellationToken)
        => _repository.ListAsync(cancellationToken);
}
