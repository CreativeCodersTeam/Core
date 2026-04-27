using CreativeCoders.Cli.Commands.Options;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Output;

/// <summary>
/// Renders a command result of type <typeparamref name="TResult"/> in a specific
/// <see cref="OutputFormat"/>.
/// </summary>
/// <typeparam name="TResult">The result type to render.</typeparam>
[PublicAPI]
public interface IOutputFormatter<in TResult>
{
    /// <summary>
    /// Gets the output format produced by this formatter.
    /// </summary>
    OutputFormat Format { get; }

    /// <summary>
    /// Writes <paramref name="value"/> to <paramref name="console"/>.
    /// </summary>
    /// <param name="value">The value to render.</param>
    /// <param name="console">The console to write to.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task WriteAsync(TResult value, IAnsiConsole console, CancellationToken cancellationToken);
}
