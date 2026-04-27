using System.Text.Json;
using CreativeCoders.Cli.Commands.Options;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Output;

/// <summary>
/// Renders a result as indented, camelCase JSON via <see cref="System.Text.Json.JsonSerializer"/>.
/// </summary>
/// <typeparam name="TResult">The result type to render.</typeparam>
[PublicAPI]
public sealed class JsonOutputFormatter<TResult> : IOutputFormatter<TResult>
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly JsonSerializerOptions _options;

    /// <summary>
    /// Initializes a new instance of <see cref="JsonOutputFormatter{TResult}"/> using default
    /// serializer options (camelCase, indented).
    /// </summary>
    public JsonOutputFormatter() : this(DefaultOptions) { }

    /// <summary>
    /// Initializes a new instance of <see cref="JsonOutputFormatter{TResult}"/> with the given
    /// serializer options.
    /// </summary>
    /// <param name="options">The serializer options to use.</param>
    public JsonOutputFormatter(JsonSerializerOptions options)
    {
        _options = Ensure.NotNull(options);
    }

    /// <inheritdoc />
    public OutputFormat Format => OutputFormat.Json;

    /// <inheritdoc />
    public Task WriteAsync(TResult value, IAnsiConsole console, CancellationToken cancellationToken)
    {
        Ensure.NotNull(console);

        var json = JsonSerializer.Serialize(value, _options);

        console.WriteLine(json);

        return Task.CompletedTask;
    }
}
