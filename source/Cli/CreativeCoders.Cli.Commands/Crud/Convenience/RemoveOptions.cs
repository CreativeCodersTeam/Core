using CreativeCoders.Cli.Commands.Options;
using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Crud.Convenience;

/// <summary>
/// Pre-built options class for remove-style commands. Implements
/// <see cref="IKeyedOptions{TKey}"/>, <see cref="IConfirmableOptions"/>,
/// <see cref="IDryRunOptions"/>, and <see cref="IVerbosityOptions"/>.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
[PublicAPI]
public class RemoveOptions<TKey> : IKeyedOptions<TKey>, IConfirmableOptions,
    IDryRunOptions, IVerbosityOptions
    where TKey : notnull
{
    /// <inheritdoc />
    public TKey Key { get; set; } = default!;

    /// <inheritdoc />
    public bool Yes { get; set; }

    /// <inheritdoc />
    public bool DryRun { get; set; }

    /// <inheritdoc />
    public Verbosity Verbosity { get; set; } = Verbosity.Normal;
}
