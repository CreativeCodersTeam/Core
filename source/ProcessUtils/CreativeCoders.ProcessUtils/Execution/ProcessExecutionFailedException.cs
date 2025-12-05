namespace CreativeCoders.ProcessUtils.Execution;

public class ProcessExecutionFailedException(int exitCode, string? errorOutput, string? message = null)
    : Exception(message ?? $"Process execution failed with exit code {exitCode}. Error output: {errorOutput}")
{
    public int ExitCode { get; } = exitCode;

    public string ErrorOutput { get; } = errorOutput ?? string.Empty;
}
