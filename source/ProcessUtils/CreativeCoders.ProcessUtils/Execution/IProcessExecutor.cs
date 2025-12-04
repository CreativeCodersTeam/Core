using JetBrains.Annotations;

namespace CreativeCoders.ProcessUtils.Execution;

[PublicAPI]
public interface IProcessExecutor<T>
{
    T? Execute();

    Task<T?> ExecuteAsync();

    ProcessExecutionResult<T?> ExecuteEx();

    Task<ProcessExecutionResult<T?>> ExecuteExAsync();
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
