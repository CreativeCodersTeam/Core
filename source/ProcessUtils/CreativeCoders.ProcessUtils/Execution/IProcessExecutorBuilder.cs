namespace CreativeCoders.ProcessUtils.Execution;

public interface IProcessExecutorBuilder<T>
{
    IProcessExecutorBuilder<T> SetFileName(string fileName);

    IProcessExecutorBuilder<T> SetArguments(string[] arguments);
    
    IProcessExecutorBuilder<T> SetOutputParser(IProcessOutputParser<T> parser);
    
    IProcessExecutor<T> Build();
}

public interface IProcessExecutorBuilder
{
    IProcessExecutorBuilder SetFileName(string fileName);

    IProcessExecutorBuilder SetArguments(string[] arguments);
    
    IProcessExecutor Build();
}