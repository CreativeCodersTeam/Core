namespace CreativeCoders.ProcessUtils.Execution;

public interface IProcessOutputParser<out T>
{
    T? ParseOutput(string? output);
}
