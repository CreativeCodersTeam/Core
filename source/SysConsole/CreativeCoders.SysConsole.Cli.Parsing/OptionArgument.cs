namespace CreativeCoders.SysConsole.Cli.Parsing;

public class OptionArgument
{
    public OptionArgumentKind Kind { get; init; }

    public string? OptionName { get; init; }

    public string? Value { get; init; }

    public bool IsProcessed { get; set; }
}
