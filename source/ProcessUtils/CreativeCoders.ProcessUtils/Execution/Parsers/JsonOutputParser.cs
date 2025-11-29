namespace CreativeCoders.ProcessUtils.Execution.Parsers;

public class JsonOutputParser<T> : IProcessOutputParser<T>
{
    public T? ParseOutput(string? output)
    {
        if (string.IsNullOrWhiteSpace(output))
        {
            return default;
        }
        
        var data = System.Text.Json.JsonSerializer
            .Deserialize<T>(output);
        
        return data;
    }
}