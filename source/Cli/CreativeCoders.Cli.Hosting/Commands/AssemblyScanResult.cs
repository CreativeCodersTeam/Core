using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Commands;

/// <summary>
/// Represents the result of scanning assemblies for CLI commands.
/// </summary>
public class AssemblyScanResult
{
    /// <summary>
    /// Gets the collection of discovered command information objects.
    /// </summary>
    /// <value>An enumerable of <see cref="CliCommandInfo"/> instances.</value>
    public required IEnumerable<CliCommandInfo> CommandInfos { get; init; }

    /// <summary>
    /// Gets the collection of command group attributes found in the scanned assemblies.
    /// </summary>
    /// <value>An enumerable of <see cref="CliCommandGroupAttribute"/> instances.</value>
    public required IEnumerable<CliCommandGroupAttribute> GroupAttributes { get; init; }
}
