using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

/// <summary>
/// Provides the default implementation of <see cref="ICliCommandContext"/>.
/// </summary>
[PublicAPI]
public class CliCommandContext : ICliCommandContext
{
    /// <inheritdoc />
    public string[] AllArgs { get; set; } = [];

    /// <inheritdoc />
    public string[] OptionsArgs { get; set; } = [];
}
