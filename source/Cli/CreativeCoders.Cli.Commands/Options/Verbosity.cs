using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Options;

/// <summary>
/// Verbosity level reported by an <see cref="IVerbosityOptions"/>.
/// </summary>
[PublicAPI]
public enum Verbosity
{
    /// <summary>Suppress informational output; only errors are written.</summary>
    Quiet,

    /// <summary>Default level; informational and error output are written.</summary>
    Normal,

    /// <summary>Verbose output, including diagnostic detail.</summary>
    Verbose
}
