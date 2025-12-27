using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.ProcessUtils.Execution.Parsers;

public class SplitLinesOutputParser : IProcessOutputParser<string[]>
{
    [SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
    public string[]? ParseOutput(string? output)
    {
        if (string.IsNullOrEmpty(output))
        {
            return [];
        }

        var lines = output.Split(Separators, SplitOptions);

        return TrimLines
            ? lines.Select(x => x.Trim()).ToArray()
            : lines;
    }

    public string[] Separators { get; set; } = [Environment.NewLine];

    public bool TrimLines { get; set; }

    public StringSplitOptions SplitOptions { get; set; } = StringSplitOptions.None;
}
