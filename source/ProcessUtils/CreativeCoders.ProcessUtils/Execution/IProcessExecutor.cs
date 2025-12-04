using JetBrains.Annotations;

namespace CreativeCoders.ProcessUtils.Execution;

[PublicAPI]
public interface IProcessExecutor<T>
{
    T? Execute();

    T? Execute(string[] args);

    Task<T?> ExecuteAsync();

    Task<T?> ExecuteAsync(string[] args);

    ProcessExecutionResult<T?> ExecuteEx();

    ProcessExecutionResult<T?> ExecuteEx(string[] args);

    Task<ProcessExecutionResult<T?>> ExecuteExAsync();

    Task<ProcessExecutionResult<T?>> ExecuteExAsync(string[] args);
}

[PublicAPI]
public interface IProcessExecutor
{
    void Execute();

    Task ExecuteAsync();

    IProcess ExecuteEx();

    Task<IProcess> ExecuteExAsync();

    int ExecuteAndReturnExitCode();

    Task<int> ExecuteAndReturnExitCodeAsync();
}
