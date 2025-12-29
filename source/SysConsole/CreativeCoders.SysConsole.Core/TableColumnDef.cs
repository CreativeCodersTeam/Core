using System;
using CreativeCoders.Core;
using Spectre.Console;

namespace CreativeCoders.SysConsole.Core;

public class TableColumnDef<T>(
    Func<T, object?> valueSelector,
    string? title = null,
    int? width = null,
    Action<TableColumn>? configureColumn = null)
{
    private readonly Func<T, object?> _valueSelector = Ensure.NotNull(valueSelector);

    public string GetValue(T item) => _valueSelector(item)?.ToString() ?? string.Empty;

    public void ConfigureColumn(TableColumn column) => configureColumn?.Invoke(column);

    public string? Title { get; } = title;

    public int? Width { get; } = width;
}
