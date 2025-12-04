namespace CreativeCoders.ProcessUtils.Execution;

public sealed class ProcessExecutionResult<T>(IProcess process, T value) : IDisposable
{
    public T Value { get; } = value;

    public IProcess Process { get; } = process;

    public void Dispose()
    {
        Process.Dispose();
    }
}
