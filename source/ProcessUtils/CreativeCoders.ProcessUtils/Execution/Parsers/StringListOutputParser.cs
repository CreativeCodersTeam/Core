namespace CreativeCoders.ProcessUtils.Execution.Parsers;

public class StringListOutputParser : IProcessOutputParser<string[]>
{
    public string[]? ParseOutput(string? output)
    {
        if (string.IsNullOrEmpty(output))
        {
            return [];
        }
        
        var lines = output
            .Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        
        return lines;
    }
}