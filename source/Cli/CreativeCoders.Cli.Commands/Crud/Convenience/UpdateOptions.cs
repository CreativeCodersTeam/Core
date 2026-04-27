using CreativeCoders.Cli.Commands.Options;
using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Crud.Convenience;

/// <summary>
/// Pre-built options class for update-style commands. Implements
/// <see cref="IKeyedOptions{TKey}"/>, <see cref="IEntityInputOptions{TEntity}"/>,
/// <see cref="IConfirmableOptions"/>, <see cref="IDryRunOptions"/>,
/// <see cref="IOutputFormatOptions"/>, and <see cref="IVerbosityOptions"/>.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TEntity">The entity type.</typeparam>
[PublicAPI]
public class UpdateOptions<TKey, TEntity> : IKeyedOptions<TKey>, IEntityInputOptions<TEntity>,
    IConfirmableOptions, IDryRunOptions, IOutputFormatOptions, IVerbosityOptions
    where TKey : notnull
{
    /// <inheritdoc />
    public TKey Key { get; set; } = default!;

    /// <inheritdoc />
    public TEntity Entity { get; set; } = default!;

    /// <inheritdoc />
    public bool Yes { get; set; }

    /// <inheritdoc />
    public bool DryRun { get; set; }

    /// <inheritdoc />
    public OutputFormat Format { get; set; } = OutputFormat.Table;

    /// <inheritdoc />
    public Verbosity Verbosity { get; set; } = Verbosity.Normal;
}
