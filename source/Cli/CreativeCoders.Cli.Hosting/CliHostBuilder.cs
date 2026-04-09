using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Cli.Hosting;

/// <summary>
/// Provides a static factory for creating <see cref="ICliHostBuilder"/> instances.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CliHostBuilder
{
    /// <summary>
    /// Creates a new instance of the default <see cref="ICliHostBuilder"/>.
    /// </summary>
    /// <returns>A new <see cref="ICliHostBuilder"/> instance.</returns>
    public static ICliHostBuilder Create()
    {
        return new DefaultCliHostBuilder();
    }
}
