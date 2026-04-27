using CreativeCoders.Cli.Commands.Options;
using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Crud.Convenience;

/// <summary>
/// Pre-built options class for list-style commands. Implements
/// <see cref="IOutputFormatOptions"/> and <see cref="IVerbosityOptions"/>.
/// </summary>
[PublicAPI]
public class ListOptions : IOutputFormatOptions, IVerbosityOptions
{
    /// <inheritdoc />
    public OutputFormat Format { get; set; } = OutputFormat.Table;

    /// <inheritdoc />
    public Verbosity Verbosity { get; set; } = Verbosity.Normal;
}
