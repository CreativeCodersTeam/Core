using CreativeCoders.Cli.Commands.Confirmation;
using CreativeCoders.Cli.Commands.Interaction;
using CreativeCoders.Cli.Commands.Output;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Base class for "list" commands. Returns a sequence of entities; output is formatted automatically
/// when <typeparamref name="TOptions"/> implements
/// <see cref="Options.IOutputFormatOptions"/>.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TOptions">The options type.</typeparam>
[PublicAPI]
public abstract class ListCommandBase<TEntity, TOptions>
    : CliCommandBase<TOptions, IReadOnlyList<TEntity>>
    where TOptions : class
{
    /// <summary>Initializes a new instance of <see cref="ListCommandBase{TEntity, TOptions}"/>.</summary>
    protected ListCommandBase(
        IConfirmationPrompt confirmationPrompt,
        IInteractivePrompter interactivePrompter,
        IAnsiConsole console,
        IEnumerable<IOutputFormatter<IReadOnlyList<TEntity>>> formatters)
        : base(confirmationPrompt, interactivePrompter, console, formatters) { }

    /// <inheritdoc />
    protected override Task<IReadOnlyList<TEntity>> OnExecuteWithResultAsync(TOptions options,
        CancellationToken cancellationToken)
        => LoadAsync(options, cancellationToken);

    /// <summary>Loads the entities to list.</summary>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The entities to list.</returns>
    protected abstract Task<IReadOnlyList<TEntity>> LoadAsync(TOptions options,
        CancellationToken cancellationToken);

    /// <inheritdoc />
    protected override bool RequiresConfirmation(TOptions options) => false;
}
