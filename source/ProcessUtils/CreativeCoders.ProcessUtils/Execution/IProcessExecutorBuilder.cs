using JetBrains.Annotations;

namespace CreativeCoders.ProcessUtils.Execution;

[PublicAPI]
public interface IProcessExecutorBuilder<T>
{
    IProcessExecutorBuilder<T> SetFileName(string fileName);

    IProcessExecutorBuilder<T> SetArguments(string[] arguments);

    IProcessExecutorBuilder<T> SetOutputParser(IProcessOutputParser<T> parser);

    IProcessExecutorBuilder<T> SetOutputParser<TParser>(Action<TParser>? configure = null)
        where TParser : IProcessOutputParser<T>, new();

    IProcessExecutor<T> Build();
}

[PublicAPI]
public interface IProcessExecutorBuilder
{
    IProcessExecutorBuilder SetFileName(string fileName);

    IProcessExecutorBuilder SetArguments(string[] arguments);

    IProcessExecutor Build();
}
