using JetBrains.Annotations;

namespace CreativeCoders.ProcessUtils.Execution;

[PublicAPI]
public interface IProcessExecutor<T>
{
    T? Execute();

    T? Execute(string[] args);

    T? Execute(IDictionary<string, object?> placeholderVars);

    Task<T?> ExecuteAsync();

    Task<T?> ExecuteAsync(string[] args);

    Task<T?> ExecuteAsync(IDictionary<string, object?> placeholderVars);

    ProcessExecutionResult<T?> ExecuteEx();

    ProcessExecutionResult<T?> ExecuteEx(string[] args);

    ProcessExecutionResult<T?> ExecuteEx(IDictionary<string, object?> placeholderVars);

    Task<ProcessExecutionResult<T?>> ExecuteExAsync();

    Task<ProcessExecutionResult<T?>> ExecuteExAsync(string[] args);

    Task<ProcessExecutionResult<T?>> ExecuteExAsync(IDictionary<string, object?> placeholderVars);
}

[PublicAPI]
public interface IProcessExecutor
{
    void Execute();

    void Execute(string[] args);

    void Execute(IDictionary<string, object?> placeholderVars);

    Task ExecuteAsync();

    Task ExecuteAsync(string[] args);

    Task ExecuteAsync(IDictionary<string, object?> placeholderVars);

    IProcess ExecuteEx();

    IProcess ExecuteEx(string[] args);
    IProcess ExecuteEx(IDictionary<string, object?> placeholderVars);

    Task<IProcess> ExecuteExAsync();

    Task<IProcess> ExecuteExAsync(string[] args);

    Task<IProcess> ExecuteExAsync(IDictionary<string, object?> placeholderVars);

    int ExecuteAndReturnExitCode();

    int ExecuteAndReturnExitCode(string[] args);

    int ExecuteAndReturnExitCode(IDictionary<string, object?> placeholderVars);

    Task<int> ExecuteAndReturnExitCodeAsync();

    Task<int> ExecuteAndReturnExitCodeAsync(string[] args);

    Task<int> ExecuteAndReturnExitCodeAsync(IDictionary<string, object?> placeholderVars);
}
