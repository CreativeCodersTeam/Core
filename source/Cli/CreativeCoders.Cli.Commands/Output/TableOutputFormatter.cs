using System.Collections;
using System.Reflection;
using CreativeCoders.Cli.Commands.Options;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Commands.Output;

/// <summary>
/// Renders a result as a Spectre.Console <see cref="Table"/>. For sequences each item becomes a
/// row, with one column per public readable property. For scalars a single-row table is rendered.
/// </summary>
/// <typeparam name="TResult">The result type to render.</typeparam>
[PublicAPI]
public sealed class TableOutputFormatter<TResult> : IOutputFormatter<TResult>
{
    /// <inheritdoc />
    public OutputFormat Format => OutputFormat.Table;

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
            RenderEnumerable(enumerable, console);
        }
        else
        {
            RenderSingle(value, console);
        }

        return Task.CompletedTask;
    }

    private static void RenderEnumerable(IEnumerable enumerable, IAnsiConsole console)
    {
        var items = enumerable.Cast<object?>().ToList();

        if (items.Count == 0)
        {
            console.MarkupLine("[grey](no items)[/]");

            return;
        }

        var itemType = items[0]?.GetType() ?? typeof(object);
        var properties = GetReadableProperties(itemType);

        if (properties.Length == 0)
        {
            // Fallback: single-column table with ToString().
            var simple = new Table().AddColumn("Value");

            foreach (var item in items)
            {
                simple.AddRow(Markup.Escape(item?.ToString() ?? string.Empty));
            }

            console.Write(simple);

            return;
        }

        var table = new Table();

        foreach (var property in properties)
        {
            table.AddColumn(property.Name);
        }

        foreach (var item in items)
        {
            var cells = properties
                .Select(p => Markup.Escape(p.GetValue(item)?.ToString() ?? string.Empty))
                .ToArray();

            table.AddRow(cells);
        }

        console.Write(table);
    }

    private static void RenderSingle(object value, IAnsiConsole console)
    {
        var properties = GetReadableProperties(value.GetType());

        if (properties.Length == 0)
        {
            console.WriteLine(value.ToString() ?? string.Empty);

            return;
        }

        var table = new Table()
            .AddColumn("Property")
            .AddColumn("Value");

        foreach (var property in properties)
        {
            table.AddRow(
                Markup.Escape(property.Name),
                Markup.Escape(property.GetValue(value)?.ToString() ?? string.Empty));
        }

        console.Write(table);
    }

    private static PropertyInfo[] GetReadableProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
            .ToArray();
    }
}
