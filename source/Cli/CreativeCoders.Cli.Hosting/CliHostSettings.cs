namespace CreativeCoders.Cli.Hosting;

/// <summary>
/// Holds configuration settings for the CLI host.
/// </summary>
public class CliHostSettings
{
    /// <summary>
    /// Gets a value indicating whether command options validation is enabled.
    /// </summary>
    /// <value><see langword="true"/> if validation is enabled; otherwise, <see langword="false"/>. The default is <see langword="false"/>.</value>
    public bool UseValidation { get; init; }
}
