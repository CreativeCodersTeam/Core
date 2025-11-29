namespace CreativeCoders.ProcessUtils.Execution;

public interface IProcessExecutor<T>
{
    T? Execute();
    
    Task<T?> ExecuteAsync();
}

public interface IProcessExecutor
{
    void Execute();

    Task ExecuteAsync();
}