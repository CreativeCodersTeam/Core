using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Options;

/// <summary>
/// Output format selected by an <see cref="IOutputFormatOptions"/>.
/// </summary>
[PublicAPI]
public enum OutputFormat
{
    /// <summary>Render the result as a Spectre.Console table. The default.</summary>
    Table,

    /// <summary>Render the result as JSON via System.Text.Json.</summary>
    Json,

    /// <summary>Render the result as plain text (one line per item for sequences).</summary>
    Plain
}
