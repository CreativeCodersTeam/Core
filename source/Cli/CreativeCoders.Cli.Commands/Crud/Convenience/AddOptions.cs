using CreativeCoders.Cli.Commands.Options;
using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Crud.Convenience;

/// <summary>
/// Pre-built options class for add-style commands. Implements
/// <see cref="IEntityInputOptions{TEntity}"/>, <see cref="IDryRunOptions"/>,
/// <see cref="IOutputFormatOptions"/>, and <see cref="IVerbosityOptions"/>.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
[PublicAPI]
public class AddOptions<TEntity> : IEntityInputOptions<TEntity>, IDryRunOptions,
    IOutputFormatOptions, IVerbosityOptions
{
    /// <inheritdoc />
    public TEntity Entity { get; set; } = default!;

    /// <inheritdoc />
    public bool DryRun { get; set; }

    /// <inheritdoc />
    public OutputFormat Format { get; set; } = OutputFormat.Table;

    /// <inheritdoc />
    public Verbosity Verbosity { get; set; } = Verbosity.Normal;
}
