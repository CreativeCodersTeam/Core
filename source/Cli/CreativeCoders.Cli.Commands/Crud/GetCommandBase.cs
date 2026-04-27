using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using CreativeCoders.Cli.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Base class for "get" commands. Returns a single entity by key. When the entity is
/// <see langword="null"/>, the command returns <see cref="ExitCodes.NotFound"/>.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TOptions">The options type. Must implement <see cref="IKeyedOptions{TKey}"/>.</typeparam>
[PublicAPI]
public abstract class GetCommandBase<TEntity, TKey, TOptions>
    : CliCommandBase<TOptions, TEntity?>
    where TOptions : class, IKeyedOptions<TKey>
{
    /// <summary>Initializes a new instance of <see cref="GetCommandBase{TEntity, TKey, TOptions}"/>.</summary>
    protected GetCommandBase(
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<TEntity?>> formatters)
        : base(confirmationPrompt, interactivePrompter, console, formatters) { }

    /// <inheritdoc />
    protected override Task<TEntity?> OnExecuteWithResultAsync(TOptions options,
        CancellationToken cancellationToken)
        => LoadByKeyAsync(options.Key, cancellationToken);

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

    /// <summary>Loads the entity with the given key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The entity, or <see langword="null"/> if not found.</returns>
    protected abstract Task<TEntity?> LoadByKeyAsync(TKey key, CancellationToken cancellationToken);

    /// <inheritdoc />
    protected override bool RequiresConfirmation(TOptions options) => false;
}
