using JetBrains.Annotations;

namespace CreativeCoders.ProcessUtils.Execution;

[PublicAPI]
public interface IProcessExecutor<T>
{
    T? Execute();

    Task<T?> ExecuteAsync();
}

[PublicAPI]
public interface IProcessExecutor
{
    void Execute();

    Task ExecuteAsync();
}
