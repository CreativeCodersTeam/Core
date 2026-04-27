using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Options;

/// <summary>
/// Marker interface implemented by option types that carry an <see cref="OutputFormat"/> setting.
/// Triggers automatic formatting of the command result by the base class.
/// </summary>
[PublicAPI]
public interface IOutputFormatOptions
{
    /// <summary>
    /// Gets or sets the desired output format. When stdout is redirected and the value is the
    /// default <see cref="OutputFormat.Table"/>, the base class falls back to
    /// <see cref="OutputFormat.Plain"/> to keep the output pipe-safe.
    /// </summary>
    OutputFormat Format { get; set; }
}
