namespace CreativeCoders.ProcessUtils.Execution.Parsers;

public class PassThroughProcessOutputParser : IProcessOutputParser<string>
{
    public string? ParseOutput(string? output)
    {
        return output;
    }
}