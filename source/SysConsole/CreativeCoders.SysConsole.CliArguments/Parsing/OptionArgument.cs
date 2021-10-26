namespace CreativeCoders.SysConsole.CliArguments.Parsing
{
    public class OptionArgument
    {
        public OptionArgumentKind Kind { get; set; }
        public string? OptionName { get; init; }

        public string? Value { get; init; }
    }
}
