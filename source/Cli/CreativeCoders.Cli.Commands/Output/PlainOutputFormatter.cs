using System.Collections;
using CreativeCoders.Cli.Commands.Options;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Output;

/// <summary>
/// Renders a result as plain text. Sequences write one line per item using
/// <see cref="object.ToString"/>; scalars write their string representation directly.
/// Used as the pipe-safe fallback when stdout is redirected.
/// </summary>
/// <typeparam name="TResult">The result type to render.</typeparam>
[PublicAPI]
public sealed class PlainOutputFormatter<TResult> : IOutputFormatter<TResult>
{
    /// <inheritdoc />
    public OutputFormat Format => OutputFormat.Plain;

    /// <inheritdoc />
    public Task WriteAsync(TResult value, IAnsiConsole console, CancellationToken cancellationToken)
    {
        Ensure.NotNull(console);

        if (value is null)
        {
            return Task.CompletedTask;
        }

        if (value is IEnumerable enumerable and not string)
        {
            foreach (var item in enumerable)
            {
                console.WriteLine(item?.ToString() ?? string.Empty);
            }
        }
        else
        {
            console.WriteLine(value.ToString() ?? string.Empty);
        }

        return Task.CompletedTask;
    }
}
